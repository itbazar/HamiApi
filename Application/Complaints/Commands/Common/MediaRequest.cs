using Domain.Models.Common;

namespace Application.Complaints.Commands.Common;
public record MediaRequest(string Title, MediaType MediaType, byte[] Data);
