using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.RefreshCommand;

internal sealed class RefreshCommandHandler(
    IAuthenticationService authenticationService) : IRequestHandler<RefreshCommand, Result<AuthToken>>
{
    public async Task<Result<AuthToken>> Handle(
        RefreshCommand request,
        CancellationToken cancellationToken)
    {
        var result = await authenticationService.Refresh(request.Token, request.RefreshToken);
        return result;
    }
}

