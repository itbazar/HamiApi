using Application.Common.Interfaces.Persistence;
using Domain.Models.WebContents;

namespace Application.WebContents.Queries.GetAdminWebContentsQuery;

internal sealed class GetAdminWebContentsQueryHandler(IWebContentRepository webContentRepository) 
    : IRequestHandler<GetAdminWebContentsQuery, Result<List<WebContent>>>
{
    public async Task<Result<List<WebContent>>> Handle(GetAdminWebContentsQuery request, CancellationToken cancellationToken)
    {
        var result = await webContentRepository.GetAsync();
        return result.ToList();
    }
}
