using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Common;

public class Media
{
    public Guid Id { get; set; }
    [NotMapped]
    public byte[] Data { get; set; } = null!;
    public byte[] Cipher { get; set; } = null!;
    public byte[] IntegrityHash { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public MediaType MediaType { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public Guid ComplaintContentId { get; set; }
    public Guid ComplaintContent { get; set; }
}

