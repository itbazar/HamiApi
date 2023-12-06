using Application.Complaints.Queries.Common;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintCitizenQuery;

public record GetComplaintCitizenQuery(string TrackingNumber, string Password) : IRequest<ComplaintResponse>;