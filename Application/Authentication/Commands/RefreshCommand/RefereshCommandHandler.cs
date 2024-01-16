using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.RefreshCommand;

internal sealed class RefreshCommandHandler : IRequestHandler<RefreshCommand, AuthToken>
{
    private readonly IAuthenticationService _authenticationService;

    public RefreshCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    public async Task<AuthToken> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.Refresh(request.Token, request.RefreshToken);
        return result;
    }
}

