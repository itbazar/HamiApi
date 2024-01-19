using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Queries.CaptchaQuery;

internal sealed class CaptchaQueryHandler(ICaptchaProvider captchaProvider) : IRequestHandler<CaptchaQuery, Result<CaptchaResultModel>>
{
    public async Task<Result<CaptchaResultModel>> Handle(
        CaptchaQuery request,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return captchaProvider.GenerateImage();
    }
}
