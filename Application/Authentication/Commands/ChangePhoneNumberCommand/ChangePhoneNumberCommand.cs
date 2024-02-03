using Application.Common.Interfaces.Security;

namespace Application.Authentication.Commands.ChangePhoneNumberCommand;

public sealed record ChangePhoneNumberCommand(
    string Username,
    string OtpToken1,
    string Code1,
    string OtpToken2,
    string Code2) : IRequest<Result<bool>>;
