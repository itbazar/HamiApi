using Application.Common.Interfaces.Security;

namespace Application.Authentication.Queries.ForgotPasswordQuery;

public sealed record ForgotPasswordQuery(
    string Username,
    CaptchaValidateModel? CaptchaValidateModel = null) : IRequest<Result<VerificationToken>>;
