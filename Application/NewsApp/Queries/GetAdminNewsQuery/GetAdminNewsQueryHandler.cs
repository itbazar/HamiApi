using Application.Common.Interfaces.Persistence;
using Domain.Models.News;
using MediatR;

namespace Application.NewsApp.Queries.GetAdminNewsQuery;

internal class GetAdminNewssQueryHandler : 
    IRequestHandler<GetAdminNewsQuery, Result<PagedList<News>>>
{
    private readonly INewsRepository _newsRepository;

    public GetAdminNewssQueryHandler(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<Result<PagedList<News>>> Handle(GetAdminNewsQuery request, CancellationToken cancellationToken)
    {
        var news = await _newsRepository.GetPagedAsync(
            request.PagingInfo,
            null,
            false,
            s => s.OrderByDescending(o => o.DateTime));
        return news;
    }
}