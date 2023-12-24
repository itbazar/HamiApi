using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.LogisterCitizenCommand;

public sealed record LogisterCitizenCommand(
    string PhoneNumber,
    string? VerificationCode = null,
    CaptchaValidateModel? CaptchaValidateModel = null) : IRequest<LoginResultModel>;
