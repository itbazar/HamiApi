using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.RevokeCommand;

internal sealed class RevokeCommandHandler(
    IAuthenticationService authenticationService) : IRequestHandler<RevokeCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        RevokeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await authenticationService.Revoke(request.UserId, request.RefreshToken);
        return result;
    }
}

