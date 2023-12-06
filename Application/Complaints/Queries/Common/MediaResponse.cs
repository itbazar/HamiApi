using Domain.Models.Common;

namespace Application.Complaints.Queries.Common;
public record MediaResponse(string Title, MediaType MediaType, byte[] Data);
