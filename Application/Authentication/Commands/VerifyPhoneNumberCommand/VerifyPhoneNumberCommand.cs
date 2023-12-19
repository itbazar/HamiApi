using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.VerifyPhoneNumberCommand;

public sealed record VerifyPhoneNumberCommand(string Username, string verificationCode, CaptchaValidateModel? CaptchaValidateModel = null) : IRequest<bool>;
