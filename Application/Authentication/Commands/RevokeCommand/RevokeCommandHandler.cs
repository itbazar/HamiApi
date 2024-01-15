using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.RevokeCommand;

internal sealed class RevokeCommandHandler : IRequestHandler<RevokeCommand, bool>
{
    private readonly IAuthenticationService _authenticationService;

    public RevokeCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    public async Task<bool> Handle(RevokeCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.Revoke(request.UserId, request.RefreshToken);
        return result;
    }
}

