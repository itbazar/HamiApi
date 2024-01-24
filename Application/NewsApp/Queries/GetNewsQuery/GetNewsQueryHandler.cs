using Application.Common.Interfaces.Persistence;
using Domain.Models.News;
using MediatR;

namespace Application.NewsApp.Queries.GetNewsQuery;

internal class GetNewssQueryHandler : IRequestHandler<GetNewsQuery, Result<PagedList<News>>>
{
    private readonly INewsRepository _newsRepository;

    public GetNewssQueryHandler(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<Result<PagedList<News>>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        var news = await _newsRepository.GetPagedAsync(
            request.PagingInfo,
            s => s.IsDeleted == false,
            false,
            s => s.OrderByDescending(o => o.DateTime));
        return news;
    }
}