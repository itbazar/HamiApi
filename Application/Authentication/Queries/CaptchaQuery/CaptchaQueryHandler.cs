using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Queries.CaptchaQuery;

internal sealed class CaptchaQueryHandler : IRequestHandler<CaptchaQuery, CaptchaResultModel>
{
    private readonly ICaptchaProvider _captchaProvider;

    public CaptchaQueryHandler(ICaptchaProvider captchaProvider)
    {
        _captchaProvider = captchaProvider;
    }

    public async Task<CaptchaResultModel> Handle(CaptchaQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return _captchaProvider.GenerateImage();
    }
}
