using Domain.ExtensionMethods;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate.Encryption;
using Domain.Models.ComplaintAggregate.Events;
using Domain.Models.IdentityAggregate;
using Domain.Models.PublicKeys;
using Domain.Primitives;
using FluentResults;
using SharedKernel.Errors;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace Domain.Models.ComplaintAggregate;

public class Complaint : Entity
{
    private readonly IHasher hasher = new Hasher();
    private readonly ISymmetricEncryption symmetricEncryption = new AesEncryption();
    private readonly IAsymmetricEncryption asymmetricEncryption = new RsaEncryption();
    private Complaint(Guid id) : base(id) { }
    public string TrackingNumber { get; set; } = null!;
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; } = null;
    public string Title { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public ComplaintCategory Category { get; set; } = null!;
    public string Complaining { get; set; } = string.Empty;
    public Guid ComplaintOrganizationId { get; set; }
    public ComplaintOrganization ComplaintOrganization { get; set; } = null!;
    public List<ComplaintContent> Contents { get; set; } = new List<ComplaintContent>();
    public ComplaintState Status { get; set; }
    public DateTime RegisteredAt { get; set; }
    public DateTime LastChanged { get; set; }

    // Citizen password
    [NotMapped]
    public string PlainPassword { get; set; } = string.Empty;
    public string ServerPassword { get; set; } = string.Empty;
    public Password CitizenPassword { get; set; } = null!;
    ///////////////////
    // Report password
    [NotMapped]
    public byte[] EncryptionKey { get; set; } = null!;
    public byte[] EncryptionIv { get; set; } = null!;
    public byte[] CipherKeyCitizen { get; set; } = null!;
    public Password EncryptionKeyPassword { get; set; } = null!;
    public byte[] EncryptionIvCitizen { get; set; } = null!;
    public byte[] CipherKeyInspector { get; set; } = null!;
    public Guid PublicKeyId { get; set; }
    public PublicKey PublicKey { get; set; } = null!;
    ///////////////////


    // Factory methods
    public static Result<Complaint> Register(
        string? userId,
        PublicKey publicKey,
        string title,
        string text,
        Guid categoryId,
        List<Media> medias,
        string complaining,
        Guid organizationId)
    {
        var now = DateTime.UtcNow;
        Complaint complaint = new(Guid.NewGuid());
        complaint.UserId = userId;
        complaint.PublicKey = publicKey;
        complaint.RegisteredAt = now;
        complaint.LastChanged = now;
        complaint.Status = ComplaintState.Registered;
        complaint.Title = title;
        complaint.CategoryId = categoryId;
        complaint.Complaining = complaining;
        complaint.ComplaintOrganizationId = organizationId;
        complaint.Contents.Add(
            ComplaintContent.Create(
                text,
                medias,
                Actor.Citizen,
                ComplaintOperation.Register,
                ComplaintContentVisibility.Everyone));

        complaint.generateTrackingNumber();

        complaint.generateCredentials();

        if (complaint.Contents.Count != 1)
            return ComplaintErrors.InconsistentContent;

        complaint.encryptContent();

        complaint.Raise(new ComplaintCreatedDomainEvent(
            Guid.NewGuid(),
            complaint.Id,
            complaint.TrackingNumber,
            complaint.PlainPassword,
            complaint.UserId));
        return complaint;
    }

    public Result ReplyInspector(
        string text,
        List<Media> medias,
        Actor sender,
        ComplaintOperation operation,
        ComplaintContentVisibility visibility,
        string encodedKey)
    {
        var addContentResult = addContent(text, medias, sender, operation, visibility);
        if(addContentResult.IsFailed)
            return addContentResult;
        
        var loadKeyResult = loadEncryptionKeyByInspector(encodedKey);
        if(loadKeyResult.IsFailed)
            return loadKeyResult;
        
        var encryptResult = encryptContent();
        if(encryptResult.IsFailed)
            return encryptResult;
        
        return Result.Ok();
    }

    public Result ReplyCitizen(
        string text,
        List<Media> medias,
        Actor sender,
        ComplaintOperation operation,
        ComplaintContentVisibility visibility,
        string password)
    {
        var addContentResult = addContent(text, medias, sender, operation, visibility);
        if (addContentResult.IsFailed)
            return addContentResult;
        
        password = password.Trim();
        var loadKeyResult = loadEncryptionKeyByCitizenPassword(password);
        if (loadKeyResult.IsFailed)
            return loadKeyResult;

        var encryptResult = encryptContent();
        if (encryptResult.IsFailed)
            return encryptResult;

        return Result.Ok();
    }

    public Result<bool> ChangeInspectorKey(
        string fromPrivateKey,
        string toPublicKey)
    {
        try
        {
            EncryptionKey = asymmetricEncryption.DecryptAsymmetricByPrivate(fromPrivateKey, CipherKeyInspector);
        }
        catch
        {
            return EncryptionErrors.Failed;
        }
        if (!hasher.VerifyPassword(EncryptionKey.ToBase64(), EncryptionKeyPassword))
            return EncryptionErrors.InvalidKey;
        try
        {
            CipherKeyInspector = asymmetricEncryption.EncryptAsymmetricByPublic(toPublicKey, EncryptionKey);
        }
        catch
        {
            return EncryptionErrors.Failed;
        }
        return true;
    }

    public Result GetCitizen(string password)
    {
        password = password.Trim();
        var loadKeyResult = loadEncryptionKeyByCitizenPassword(password);
        if (loadKeyResult.IsFailed)
            return loadKeyResult;

        var decryptResult = decryptContent();
        if (decryptResult.IsFailed)
            return decryptResult;

        return Result.Ok();
    }

    public Result GetInspector(string encodedKey)
    {
        var loadKeyResult = loadEncryptionKeyByInspector(encodedKey);
        if (loadKeyResult.IsFailed)
            return loadKeyResult;

        var decryptResult = decryptContent();
        if (decryptResult.IsFailed)
            return decryptResult;

        return Result.Ok();
    }

    public Result MarkAsRead(string encodedKey)
    {
        if (!ShouldMarkedAsRead())
        {
            return ComplaintErrors.InvalidOperation;
        }
        var replyResult = ReplyInspector(
            "درخواست توسط بازرس مشاهده شده و در دست بررسی است.",
            new List<Media>(),
            Actor.Inspector,
            ComplaintOperation.Open,
            ComplaintContentVisibility.Inspector,
            encodedKey);
        if(replyResult.IsFailed) 
            return replyResult;
        return Result.Ok();
    }

    public bool ShouldMarkedAsRead()
    {
        return Status == ComplaintState.Registered || Status == ComplaintState.CitizenReplied;
    }

    public List<ComplaintOperation> GetPossibleOperations(Actor actor)
    {
        return (Status, actor) switch
        {
            (ComplaintState.WaitingForCitizenResponse, Actor.Citizen) => [ComplaintOperation.CitizenReply],
            (ComplaintState.Registered, Actor.Inspector) => [ComplaintOperation.Open],
            (ComplaintState.InProgress, Actor.Inspector) =>
            [
                ComplaintOperation.AddDetails,
                ComplaintOperation.RequestForDescription,
                ComplaintOperation.Finish
            ],
            (ComplaintState.CitizenReplied, Actor.Inspector) => [ComplaintOperation.Open],
            (ComplaintState.WaitingForCitizenResponse, Actor.Inspector) => [ComplaintOperation.CancelRequest],
            (ComplaintState.Finished, Actor.Inspector) => [ComplaintOperation.StartAgain],

            _ => []
        };
    }

    private Result addContent(
        string text,
        List<Media> medias,
        Actor sender,
        ComplaintOperation operation,
        ComplaintContentVisibility visibility)
    {
        var now = DateTime.UtcNow;
        LastChanged = now;
        try
        {
            Status = (Status, sender, operation) switch
            {
                (ComplaintState.Registered, Actor.Inspector, ComplaintOperation.Open) => ComplaintState.InProgress,
                (ComplaintState.InProgress, Actor.Inspector, ComplaintOperation.RequestForDescription) => ComplaintState.WaitingForCitizenResponse,
                (ComplaintState.WaitingForCitizenResponse, Actor.Citizen, ComplaintOperation.CitizenReply) => ComplaintState.CitizenReplied,
                (ComplaintState.CitizenReplied, Actor.Inspector, ComplaintOperation.Open) => ComplaintState.InProgress,
                (ComplaintState.WaitingForCitizenResponse, Actor.Inspector, ComplaintOperation.CancelRequest) => ComplaintState.InProgress,
                (ComplaintState.InProgress, Actor.Inspector, ComplaintOperation.AddDetails) => ComplaintState.InProgress,
                (ComplaintState.InProgress, Actor.Inspector, ComplaintOperation.Finish) => ComplaintState.Finished,
                (ComplaintState.Finished, Actor.Inspector, ComplaintOperation.StartAgain) => ComplaintState.InProgress,

                _ => throw new Exception("Invalid operation")
            };
        }
        catch
        {
            return ComplaintErrors.InvalidOperation;
        }
        var content = ComplaintContent.Create(text, medias, sender, operation, visibility);
        Contents.Add(content);

        Raise(new ComplaintUpdatedDomainEvent(Guid.NewGuid(), Id, TrackingNumber, sender, Status, UserId));

        return Result.Ok();
    }

    private void generateTrackingNumber()
    {
        TrackingNumber = RandomNumberGenerator.GetInt32(10000000, 99999999).ToString();
    }

    private Result generateCredentials()
    {
        // Generate citizen key and password
        SymmetricKeyComponents encryptionKeyCitizen = new SymmetricKeyComponents(symmetricEncryption);
        ServerPassword = encryptionKeyCitizen.PasswordServer;
        PlainPassword = encryptionKeyCitizen.PasswordCitizen;
        CitizenPassword = hasher.HashPasword(PlainPassword);
        EncryptionIvCitizen = encryptionKeyCitizen.IV;

        //Generate report encryption key
        SymmetricKeyComponents encryptionKey = new SymmetricKeyComponents(symmetricEncryption);
        EncryptionKey = encryptionKey.Key;
        EncryptionIv = encryptionKey.IV;
        EncryptionKeyPassword = hasher.HashPasword(EncryptionKey.ToBase64());

        // Store report encryption key as encrypted by citizen and inspector keys
        try
        {
            CipherKeyInspector = asymmetricEncryption.EncryptAsymmetricByPublic(PublicKey.Key, EncryptionKey);
            CipherKeyCitizen = symmetricEncryption.Encrypt(encryptionKeyCitizen, encryptionKey.Key);
        }
        catch
        {
            return EncryptionErrors.Failed;
        }
        
        return Result.Ok();
    }

    private Result encryptContent(int index = 0)
    {
        if (EncryptionKey is null || EncryptionIv is null)
            return EncryptionErrors.InvalidKey;
        var encryptResult = Contents[index].Encrypt(EncryptionKey, EncryptionIv, hasher, symmetricEncryption);
        if(encryptResult.IsFailed)
            return encryptResult;
        return Result.Ok();
    }

    private Result decryptContent()
    {
        if (EncryptionKey is null || EncryptionIv is null)
            return EncryptionErrors.InvalidKey;
        foreach (var content in Contents)
        {
            var decryptResult = content.Decrypt(EncryptionKey, EncryptionIv, hasher, symmetricEncryption);
            if(decryptResult.IsFailed)
                return decryptResult;
        }
        return Result.Ok();
    }

    private Result loadEncryptionKeyByCitizenPassword(string password)
    {
        if (!hasher.VerifyPassword(password, CitizenPassword))
            return EncryptionErrors.WrongPassword;
        PlainPassword = password;
        var citizenEncryptionKey = new SymmetricKeyComponents(password, ServerPassword, EncryptionIvCitizen);
        EncryptionKey = symmetricEncryption.Decrypt(citizenEncryptionKey, CipherKeyCitizen);
        if (!hasher.VerifyPassword(EncryptionKey.ToBase64(), EncryptionKeyPassword))
            return EncryptionErrors.InvalidKey;

        return Result.Ok();
    }

    private Result loadEncryptionKeyByInspector(string encodedKey)
    {
        EncryptionKey = encodedKey.ParseBytes();
        if (!hasher.VerifyPassword(EncryptionKey.ToBase64(), EncryptionKeyPassword))
            return EncryptionErrors.InvalidKey;

        return Result.Ok();
    }

    
}

#region Enums
public enum Actor
{
    [Description("شهروند")]
    Citizen,
    [Description("بازرس")]
    Inspector
}

public enum ComplaintState
{
    [Description("ثبت شده")]
    Registered,
    [Description("در حال رسیدگی")]
    InProgress,
    [Description("در انتظار پاسخ شهروند")]
    WaitingForCitizenResponse,
    [Description("پاسخ داده شده")]
    CitizenReplied,
    [Description("پایان یافته")]
    Finished
}

public enum ComplaintOperation
{
    [Description("ثبت")]
    Register,
    [Description("مشاهده")]
    Open,
    [Description("درخواست تکمیل گزارش")]
    RequestForDescription,
    [Description("پاسخ")]
    CitizenReply,
    [Description("انصراف از تکمیل گزارش")]
    CancelRequest,
    [Description("افزودن جزئیات")]
    AddDetails,
    [Description("خاتمه")]
    Finish,
    [Description("بازگشایی")]
    StartAgain
}

public record ComplaintMessage(Actor To, string Message);

#endregion
