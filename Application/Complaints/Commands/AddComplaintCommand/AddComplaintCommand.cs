using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.Complaints.Commands.AddComplaintCommand;

public record AddComplaintCommand(string Title, string Text, Guid CategoryId): IRequest<AddComplaintResult>;
