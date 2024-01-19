using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Queries.CaptchaQuery;

public sealed record CaptchaQuery() : IRequest<Result<CaptchaResultModel>>;

