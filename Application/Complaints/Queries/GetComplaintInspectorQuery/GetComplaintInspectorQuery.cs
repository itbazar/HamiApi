using Application.Complaints.Common;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintInspectorQuery;

public record GetComplaintInspectorQuery(string TrackingNumber, string EncodedKey) : IRequest<ComplaintResponse>;