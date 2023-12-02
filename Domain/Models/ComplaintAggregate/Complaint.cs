using Domain.Models.Common;
using Domain.Models.IdentityAggregate;
using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Domain.Models.ComplaintAggregate;

public class Complaint : Entity
{
    private Complaint(Guid id) : base(id) { }
    public string TrackingNumber { get; set; } = null!;
    [NotMapped]
    public string PlainPassword { get; set; } = string.Empty;
    public Password Password { get; set; } = null!;
    public ApplicationUser? User { get; set; } = null;

    public string Title { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public ComplaintCategory Category { get; set; } = null!;
    [NotMapped]
    public List<ComplaintContent> Contents { get; set; } = new List<ComplaintContent>();
    public List<ComplaintContentCitizen> ContentsCitizen { get; set; } = new List<ComplaintContentCitizen>();
    public List<ComplaintContentInspector> ContentsInspector { get; set; } = new List<ComplaintContentInspector>();
    public ComplaintState Status { get; set; }

    // Factory methods
    public static Complaint Register(string title, string text, Guid categoryId)
    {
        var now = DateTime.Now;
        Complaint complaint = new(Guid.NewGuid());
        complaint.Title = title;
        complaint.CategoryId = categoryId;
        complaint.Contents.Add(ComplaintContent.Create(text));

        return complaint;
    }
}

public class ComplaintContent : Entity
{
    protected ComplaintContent(Guid id) : base(id) { }
    public static ComplaintContent Create(string text)
    {
        var complaintContent = new ComplaintContent(Guid.NewGuid());
        complaintContent.Text = text;
        return complaintContent;
    }

    public static ComplaintContent Create(Guid id, string text)
    {
        var complaintContent = new ComplaintContent(id);
        complaintContent.Text = text;
        return complaintContent;
    }

    public string Text { get; set; } = string.Empty;
    [NotMapped]
    public List<Media> Media { get; set; } = new List<Media>();
    public Actor Sender { get; set; }
    public DateTime DateTime { get; set; }
    public bool IsEncrypted { get; set; }
}

public class ComplaintContentCitizen : ComplaintContent
{
    public ComplaintContentCitizen(Guid id) : base(id) { }
    public byte[] SymmetricCipher { get; set; } = null!;
    public List<MediaCitizen> MediaCitizen { get; set; } = new List<MediaCitizen>();
}

public class ComplaintContentInspector : ComplaintContent
{
    public ComplaintContentInspector(Guid id) : base(id) { }
    public byte[] AsymmetricCipher { get; set; } = null!;
    public List<MediaInspector> MediaInspector { get; set; } = new List<MediaInspector>();
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