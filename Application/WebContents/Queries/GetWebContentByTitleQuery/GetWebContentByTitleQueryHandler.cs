using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Queries.GetWebContentByTitleQuery;

internal sealed class GetWebContentByTitleQueryHandler(IWebContentRepository webContentRepository) : IRequestHandler<GetWebContentByTitleQuery, Result<WebContent>>
{
    public async Task<Result<WebContent>> Handle(GetWebContentByTitleQuery request, CancellationToken cancellationToken)
    {
        var result = await webContentRepository.GetFirstAsync(wc => wc.Title == request.Title);
        if (result is null)
            return GenericErrors.NotFound;
        return result;
    }
}