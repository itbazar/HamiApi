using Application.Common.Interfaces.Persistence;
using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Queries.GetAdminWebContentsQuery;

internal sealed class GetAdminWebContentsQueryHandler : IRequestHandler<GetAdminWebContentsQuery, List<WebContent>>
{
    private readonly IWebContentRepository _webContentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetAdminWebContentsQueryHandler(IWebContentRepository webContentRepository, IUnitOfWork unitOfWork)
    {
        _webContentRepository = webContentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<WebContent>> Handle(GetAdminWebContentsQuery request, CancellationToken cancellationToken)
    {
        var result = await _webContentRepository.GetAsync();
        return result.ToList();
    }
}
