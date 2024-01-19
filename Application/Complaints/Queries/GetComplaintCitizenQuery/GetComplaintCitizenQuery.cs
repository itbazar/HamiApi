using Application.Complaints.Common;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintCitizenQuery;

public record GetComplaintCitizenQuery(
    string TrackingNumber,
    string Password) : IRequest<Result<ComplaintCitizenResponse>>;