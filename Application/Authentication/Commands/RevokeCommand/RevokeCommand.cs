using MediatR;

namespace Application.Authentication.Commands.RevokeCommand;

public sealed record RevokeCommand(
    string UserId,
    string RefreshToken) : IRequest<Result<bool>>;
