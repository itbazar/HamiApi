using Domain.Models.Common;

namespace Application.Complaints.Queries.Common;
public record MediaResponse(string Title, string MimeType, MediaType MediaType, byte[] Data);
