using Domain.Models.Common;

namespace Application.Complaints.Common;
public record MediaResponse(string Title, string MimeType, MediaType MediaType, byte[] Data);
