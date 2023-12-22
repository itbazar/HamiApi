using Domain.Models.Common;
using Domain.Models.IdentityAggregate;
using Domain.Models.PublicKeys;
using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.ComplaintAggregate;

public class Complaint : Entity
{
    private Complaint(Guid id) : base(id) { }
    public string TrackingNumber { get; set; } = null!;
    public ApplicationUser? User { get; set; } = null;
    public string Title { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public ComplaintCategory Category { get; set; } = null!;
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
    public static Complaint Register(PublicKey publicKey, string title, string text, Guid categoryId, List<Media> medias)
    {
        var now = DateTime.UtcNow;
        Complaint complaint = new(Guid.NewGuid());
        complaint.PublicKey = publicKey;
        complaint.RegisteredAt = now;
        complaint.LastChanged = now;
        complaint.Status = ComplaintState.Registered;
        complaint.Title = title;
        complaint.CategoryId = categoryId;
        complaint.Contents.Add(ComplaintContent.Create(text, medias, Actor.Citizen, ComplaintContentVisibility.Everyone));

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
}

public enum Actor
{
    Citizen,
    Inspector
}

public enum ComplaintState
{
    Registered,
    InProgress,
    WaitingForCitizenResponse,
    CitizenReplied,
    Finished
}

public enum ComplaintOperation
{
    Register,
    Open,
    RequestForDescription,
    CitizenReply,
    CancelRequest,
    AddDetails,
    Finish,
    StartAgain
}