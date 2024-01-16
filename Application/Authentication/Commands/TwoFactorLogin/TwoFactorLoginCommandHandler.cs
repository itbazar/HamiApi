using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.LoginCommand;

internal sealed class TwoFactorLoginCommandHandler : IRequestHandler<TwoFactorLoginCommand, AuthToken>
{
    private readonly IAuthenticationService _authenticationService;

    public TwoFactorLoginCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    public async Task<AuthToken> Handle(TwoFactorLoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.VerifyOtp(request.OtpToken, request.Code);
        return result;
    }
}

