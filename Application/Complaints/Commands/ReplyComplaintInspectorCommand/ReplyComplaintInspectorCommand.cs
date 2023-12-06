using Application.Complaints.Commands.Common;
using MediatR;

namespace Application.Complaints.Commands.AddComplaintCommand;

public record ReplyComplaintInspectorCommand(
    string TrackingNumber,
    string Text,
    List<MediaRequest> Medias,
    string EncryptedKey) : IRequest<bool>;
