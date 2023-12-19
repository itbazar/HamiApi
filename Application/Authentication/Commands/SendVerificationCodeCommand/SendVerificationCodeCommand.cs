using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.SendVerificationCodeCommand;

public sealed record SendVerificationCodeCommand(string Username, CaptchaValidateModel? CaptchaValidateModel = null) : IRequest<bool>;
