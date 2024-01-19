using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.LoginCommand;

internal sealed class TwoFactorLoginCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<TwoFactorLoginCommand, Result<AuthToken>>
{
    public async Task<Result<AuthToken>> Handle(TwoFactorLoginCommand request, CancellationToken cancellationToken)
    {
        var result = await authenticationService.VerifyOtp(request.OtpToken, request.Code);
        return result;
    }
}

