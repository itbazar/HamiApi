using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.RegisterCitizenCommand;

public sealed record RegisterCitizenCommand(
    string Username,
    string Password,
    CaptchaValidateModel? CaptchaValidateModel = null) :IRequest<bool>;
