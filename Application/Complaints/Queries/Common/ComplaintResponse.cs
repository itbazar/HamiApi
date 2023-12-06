using Domain.Models.ComplaintAggregate;

namespace Application.Complaints.Queries.Common;

public record ComplaintResponse(
    string TrackingNumber,
    string Title,
    ComplaintCategoryResponse Category,
    ComplaintState Status,
    DateTime RegisteredAt,
    DateTime LastChanged,
    Actor LastActor,
    List<ComplaintContentResponse> Contents);