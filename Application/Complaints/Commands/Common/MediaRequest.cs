using Domain.Models.Common;

namespace Application.Complaints.Commands.Common;
public record MediaRequest(string Title, string MimeType, MediaType MediaType, byte[] Data);
