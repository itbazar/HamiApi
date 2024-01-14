using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.RefreshCommand;

internal sealed class RefreshCommandHandler : IRequestHandler<RefreshCommand, LoginResultModel>
{
    private readonly IAuthenticationService _authenticationService;

    public RefreshCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    public async Task<LoginResultModel> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        LoginResultModel? result;
        result = await _authenticationService.Refresh(request.Token, request.RefreshToken);
        return result;
    }
}

