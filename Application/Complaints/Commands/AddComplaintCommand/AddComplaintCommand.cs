using Application.Common.Interfaces.Security;
using Application.Complaints.Commands.Common;
using MediatR;

namespace Application.Complaints.Commands.AddComplaintCommand;

public record AddComplaintCommand(
    string? UserId,
    string Title,
    string Text,
    Guid CategoryId,
    List<MediaRequest> Medias,
    CaptchaValidateModel? CaptchaValidateModel,
    string? Complaining,
    Guid? OrganizationId) : IRequest<AddComplaintResult>;
