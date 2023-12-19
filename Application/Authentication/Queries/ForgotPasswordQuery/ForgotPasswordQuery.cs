using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Queries.ForgotPasswordQuery;

public sealed record ForgotPasswordQuery(string Username, CaptchaValidateModel? CaptchaValidateModel = null) : IRequest<bool>;
