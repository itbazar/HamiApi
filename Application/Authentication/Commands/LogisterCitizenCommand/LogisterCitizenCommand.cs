using Application.Common.Interfaces.Security;

namespace Application.Authentication.Commands.LogisterCitizenCommand;

public sealed record LogisterCitizenCommand(
    string PhoneNumber,
    CaptchaValidateModel? CaptchaValidateModel = null) : IRequest<Result<string>>;
