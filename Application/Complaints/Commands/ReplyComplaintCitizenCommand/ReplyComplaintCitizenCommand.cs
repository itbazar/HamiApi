using Application.Complaints.Commands.Common;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.Complaints.Commands.ReplyComplaintCitizenCommand;

public record ReplyComplaintCitizenCommand(
    string TrackingNumber,
    string Text,
    List<MediaRequest> Medias,
    ComplaintOperation Operation,
    string Password) : IRequest<bool>;
