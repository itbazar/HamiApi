using Domain.Models.ComplaintAggregate;

namespace Application.Complaints.Queries.Common;

public record ComplaintListResponse(
    Guid Id,
    string TrackingNumber,
    string Title,
    ComplaintCategoryResponse Category,
    ComplaintState Status,
    DateTime RegisteredAt,
    DateTime LastChanged,
    Actor LastActor,
    byte[] CipherKeyInspector);
