using Application.Complaints.Commands.Common;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.Complaints.Commands.AddComplaintCommand;

public record ReplyComplaintInspectorCommand(
    string TrackingNumber,
    string Text,
    List<MediaRequest> Medias,
    ComplaintOperation Operation,
    bool IsPublic,
    string EncodedKey) : IRequest<Result<bool>>;
