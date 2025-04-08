using Application.Common.Interfaces.Security;

namespace Application.Authentication.Queries.GetResetPasswordTokenQuery;

public sealed record GetResetPasswordTokenQuery(
    string Username, string verificationCode,
    CaptchaValidateModel? CaptchaValidateModel = null) : IRequest<Result<string>>;
