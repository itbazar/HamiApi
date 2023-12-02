using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintCitizenQuery;

public record GetComplaintCitizenQuery(string TrackingNumber, string Password) : IRequest<Complaint>;