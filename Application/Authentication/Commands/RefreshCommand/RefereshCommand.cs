using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.RefreshCommand;

public sealed record RefreshCommand(
    string Token,
    string RefreshToken) : IRequest<Result<AuthToken>>;
