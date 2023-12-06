using Domain.Models.Common;
using Domain.Models.IdentityAggregate;
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
    public Actor LastActor { get; set; }

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
    ///////////////////


    // Factory methods
    public static Complaint Register(string title, string text, Guid categoryId, List<Media> medias)
    {
        var now = DateTime.UtcNow;
        Complaint complaint = new(Guid.NewGuid());
        complaint.RegisteredAt = now;
        complaint.LastChanged = now;
        complaint.LastActor = Actor.Citizen;
        complaint.Title = title;
        complaint.CategoryId = categoryId;
        complaint.Contents.Add(ComplaintContent.Create(text, medias, Actor.Citizen));

        return complaint;
    }

    public bool Reply(string text, List<Media> medias, Actor sender)
    {
        var now = DateTime.UtcNow;
        LastChanged = now;
        LastActor = sender;
        var content = ComplaintContent.Create(text, medias, sender);
        Contents.Add(content);
        return true;
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
    Replied,
    Finished
}