using Domain.Models.Common;
using Domain.Models.ComplaintAggregate.Encryption;
using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models.ComplaintAggregate;

public class ComplaintContent : Entity
{
    protected ComplaintContent(Guid id) : base(id) { }
    public static ComplaintContent Create(string text, List<Media> medias, Actor sender, ComplaintContentVisibility visibility)
    {
        var complaintContent = new ComplaintContent(Guid.Empty);
        complaintContent.Text = text;
        complaintContent.Sender = sender;
        complaintContent.DateTime = DateTime.UtcNow;
        complaintContent.Media = medias;
        complaintContent.Visibility = visibility;
        
        return complaintContent;
    }
    public static ComplaintContent Create(Guid id, string text, DateTime dateTime, List<Media> medias, Actor sender, ComplaintContentVisibility visibility)
    {
        var complaintContent = new ComplaintContent(id);
        complaintContent.Text = text;
        complaintContent.DateTime = dateTime;
        complaintContent.Sender = sender;
        complaintContent.Media = medias;
        complaintContent.Visibility = visibility;
        return complaintContent;
    }

    [NotMapped]
    public string Text { get; set; } = string.Empty;
    public byte[] Cipher { get; set; } = null!;
    public byte[] IntegrityHash { get; set; } = null!;
    [NotMapped]
    public List<Media> Media { get; set; } = new List<Media>();
    public Actor Sender { get; set; }
    public DateTime DateTime { get; set; }
    public bool IsEncrypted { get; set; }
    public ComplaintContentVisibility Visibility { get; set; }

    #region PrivateMethods
    public bool Encrypt(
        byte[] key,
        byte[] iv,
        IHasher hasher,
        ISymmetricEncryption symmetricEncryption)
    {
        UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        var textBytes = unicodeEncoding.GetBytes(Text);
        Cipher = symmetricEncryption.Encrypt(key, iv, textBytes) ?? throw new Exception("Symmetric encryption error.");
        IntegrityHash = hasher.Hash(textBytes);
        IsEncrypted = true;

        foreach (var media in Media)
        {
            media.Cipher = symmetricEncryption.Encrypt(key, iv, media.Data);
            media.IntegrityHash = hasher.Hash(media.Data);
        }
        return true;
    }

    public bool Decrypt(
        byte[] key, byte[] iv,
        IHasher hasher,
        ISymmetricEncryption symmetricEncryption)
    {
        UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        if (Cipher is null)
            throw new Exception("Cipher is null");
        var textBytes = symmetricEncryption.Decrypt(key, iv, Cipher) ?? throw new Exception("Symmetric encryption error.");
        if (!hasher.Verify(textBytes, IntegrityHash))
            throw new Exception("Invalid hash");
        Text = unicodeEncoding.GetString(textBytes);
        IsEncrypted = false;

        foreach (var media in Media)
        {
            media.Data = symmetricEncryption.Decrypt(key, iv, media.Cipher);
            if (!hasher.Verify(media.Data, media.IntegrityHash))
                throw new Exception("Invalid hash");
        }
        return true;
    }
    #endregion
}

public enum ComplaintContentVisibility
{
    Inspector,
    Everyone
}