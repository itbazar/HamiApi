using Domain.Models.Common;
using Domain.Models.ComplaintAggregate.Encryption;
using Domain.Primitives;
using FluentResults;
using SharedKernel.Errors;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models.ComplaintAggregate;

public class ComplaintContent : Entity
{
    protected ComplaintContent(Guid id) : base(id) { }
    public static ComplaintContent Create(string text, List<Media> medias, Actor sender, ComplaintOperation operation, ComplaintContentVisibility visibility)
    {
        var complaintContent = new ComplaintContent(Guid.Empty);
        complaintContent.Text = text;
        complaintContent.Sender = sender;
        complaintContent.Operation = operation;
        complaintContent.DateTime = DateTime.UtcNow;
        complaintContent.Media = medias;
        complaintContent.Visibility = visibility;
        
        return complaintContent;
    }

    //public static ComplaintContent Create(Guid id, string text, DateTime dateTime, List<Media> medias, Actor sender, ComplaintContentVisibility visibility)
    //{
    //    var complaintContent = new ComplaintContent(id);
    //    complaintContent.Text = text;
    //    complaintContent.DateTime = dateTime;
    //    complaintContent.Sender = sender;
    //    complaintContent.Media = medias;
    //    complaintContent.Visibility = visibility;
    //    return complaintContent;
    //}

    [NotMapped]
    public string Text { get; set; } = string.Empty;
    public byte[] Cipher { get; set; } = null!;
    public byte[] IntegrityHash { get; set; } = null!;
    [NotMapped]
    public List<Media> Media { get; set; } = new List<Media>();
    public Actor Sender { get; set; }
    public ComplaintOperation Operation { get; set; }
    public DateTime DateTime { get; set; }
    public bool IsEncrypted { get; set; }
    public ComplaintContentVisibility Visibility { get; set; }

    #region PrivateMethods
    public Result Encrypt(
        byte[] key,
        byte[] iv,
        IHasher hasher,
        ISymmetricEncryption symmetricEncryption)
    {
        UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        var textBytes = unicodeEncoding.GetBytes(Text);
        Cipher = symmetricEncryption.Encrypt(key, iv, textBytes);
        if (Cipher is null)
            return EncryptionErrors.Failed;
        IntegrityHash = hasher.Hash(textBytes);
        IsEncrypted = true;

        foreach (var media in Media)
        {
            media.Cipher = symmetricEncryption.Encrypt(key, iv, media.Data);
            media.IntegrityHash = hasher.Hash(media.Data);
        }
        return Result.Ok();
    }

    public Result Decrypt(
        byte[] key, byte[] iv,
        IHasher hasher,
        ISymmetricEncryption symmetricEncryption)
    {
        UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        if (Cipher is null)
            return EncryptionErrors.NullCipher;
        var textBytes = symmetricEncryption.Decrypt(key, iv, Cipher);
        if(textBytes is null) 
            return EncryptionErrors.Failed;
        if (!hasher.Verify(textBytes, IntegrityHash))
            return EncryptionErrors.InvalidHash;

        Text = unicodeEncoding.GetString(textBytes);
        IsEncrypted = false;

        foreach (var media in Media)
        {
            media.Data = symmetricEncryption.Decrypt(key, iv, media.Cipher);
            if (!hasher.Verify(media.Data, media.IntegrityHash))
                return EncryptionErrors.InvalidHash;
        }
        return Result.Ok();
    }
    #endregion
}

public enum ComplaintContentVisibility
{
    [Description("بازرس")]
    Inspector,
    [Description("همه")]
    Everyone
}