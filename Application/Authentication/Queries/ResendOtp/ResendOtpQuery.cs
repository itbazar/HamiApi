using Application.Common.Interfaces.Security;

namespace Application.Authentication.Queries.ResendOtp;

public sealed record ResendOtpQuery(
    string OtpToken,
    CaptchaValidateModel? CaptchaValidateModel = null) : IRequest<Result<string>>;
