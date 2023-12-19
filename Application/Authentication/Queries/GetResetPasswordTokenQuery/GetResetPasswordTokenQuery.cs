using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Queries.GetResetPasswordTokenQuery;

public sealed record GetResetPasswordTokenQuery(string Username, string verificationCode, CaptchaValidateModel? CaptchaValidateModel = null) : IRequest<string>;
