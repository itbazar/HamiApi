using Domain.Models.Common;
using Domain.Models.ComplaintAggregate.Events;
using Domain.Models.IdentityAggregate;
using Domain.Models.PublicKeys;
using Domain.Primitives;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.ComplaintAggregate;

public class Complaint : Entity
{
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
    public static Complaint Register(
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
        complaint.Contents.Add(ComplaintContent.Create(text, medias, Actor.Citizen, ComplaintContentVisibility.Everyone));

        complaint.Raise(new ComplaintCreatedDomainEvent(Guid.NewGuid(), complaint.Id));
        return complaint;
    }

    public bool AddContent(
        string text,
        List<Media> medias,
        Actor sender,
        ComplaintOperation operation,
        ComplaintContentVisibility visibility)
    {
        var now = DateTime.UtcNow;
        LastChanged = now;
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
        var content = ComplaintContent.Create(text, medias, sender, visibility);
        Contents.Add(content);

        Raise(new ComplaintUpdatedDomainEvent(Guid.NewGuid(), Id));

        return true;
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
    
    public List<ComplaintMessage> GetMessages(ComplaintOperation operation)
    {
        return operation switch
        {
            ComplaintOperation.Register =>
                new List<ComplaintMessage>()
                {
                    new ComplaintMessage(
                        Actor.Citizen,
                        $"درخواست شما با کد رهگیری {PlainPassword} و رمز {TrackingNumber} ثبت شد."),
                },
            _ => new List<ComplaintMessage>()
        };
    }
}

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
    [Description("درخواست توضیحات")]
    RequestForDescription,
    [Description("پاسخ")]
    CitizenReply,
    [Description("انصراف از درخواست توضیحات")]
    CancelRequest,
    [Description("افزودن جزئیات")]
    AddDetails,
    [Description("خاتمه")]
    Finish,
    [Description("بازگشایی")]
    StartAgain
}

public record ComplaintMessage(Actor To, string Message);