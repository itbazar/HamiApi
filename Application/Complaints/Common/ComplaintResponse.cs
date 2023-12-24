using Domain.Models.ComplaintAggregate;

namespace Application.Complaints.Common;

public record ComplaintResponse(
    string TrackingNumber,
    string Title,
    ComplaintCategoryResponse Category,
    ComplaintState Status,
    DateTime RegisteredAt,
    DateTime LastChanged,
    List<ComplaintContentResponse> Contents,
    List<ComplaintOperation> PossibleOperations);
