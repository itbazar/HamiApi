using Application.Common.Interfaces.Security;

namespace Application.Authentication.Queries.ChangePhoneNumberQuery;

public sealed record ChangePhoneNumberQuery(
    string Username,
    string NewPhoneNumber,
    CaptchaValidateModel? CaptchaValidateModel = null) 
    : IRequest<Result<ChangePhoneNumberQueryResponse>>;

public record ChangePhoneNumberQueryResponse(
    string PhoneNumber,
    string Token,
    string NewPhoneNumber,
    string NewToken);