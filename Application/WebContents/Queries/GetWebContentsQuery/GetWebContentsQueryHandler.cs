using Application.Common.Interfaces.Persistence;
using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Queries.GetWebContentsQuery;

internal sealed class GetWebContentsQueryHandler(IWebContentRepository webContentRepository) : IRequestHandler<GetWebContentsQuery, Result<List<WebContent>>>
{
    public async Task<Result<List<WebContent>>> Handle(GetWebContentsQuery request, CancellationToken cancellationToken)
    {
        var result = await webContentRepository.GetAsync();
        return result.ToList();
    }
}
