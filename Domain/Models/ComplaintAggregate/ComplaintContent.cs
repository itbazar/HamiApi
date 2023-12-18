using Domain.Models.Common;
using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.ComplaintAggregate;

public class ComplaintContent : Entity
{
    protected ComplaintContent(Guid id) : base(id) { }
    public static ComplaintContent Create(string text, List<Media> medias, Actor sender, ComplaintContentVisibility visibility)
    {
        var complaintContent = new ComplaintContent(Guid.NewGuid());
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
}

public enum ComplaintContentVisibility
{
    Inspector,
    Everyone
}