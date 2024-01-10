using Application.Common.Interfaces.Persistence;
using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Queries.GetWebContentsQuery;

internal sealed class GetWebContentsQueryHandler : IRequestHandler<GetWebContentsQuery, List<WebContent>>
{
    private readonly IWebContentRepository _webContentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetWebContentsQueryHandler(IWebContentRepository webContentRepository, IUnitOfWork unitOfWork)
    {
        _webContentRepository = webContentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<WebContent>> Handle(GetWebContentsQuery request, CancellationToken cancellationToken)
    {
        var result = await _webContentRepository.GetAsync();
        return result.ToList();
    }
}
