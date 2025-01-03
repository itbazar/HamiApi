using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Common;

public class MedicalMedia
{
    public Guid Id { get; set; }
    [NotMapped]
    public byte[] Data { get; set; } = null!;
    public byte[] Cipher { get; set; } = null!;
    public byte[] IntegrityHash { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public MediaType MediaType { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public Guid UserMedicalInfoId { get; set; }
    public Guid UserMedicalInfo { get; set; }
}

