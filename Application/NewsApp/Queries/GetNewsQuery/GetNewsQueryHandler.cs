using Application.Common.Interfaces.Persistence;
using Domain.Models.News;
using MediatR;

namespace Application.NewsApp.Queries.GetNewsQuery;

internal class GetNewssQueryHandler : IRequestHandler<GetNewsQuery, Result<List<News>>>
{
    private readonly INewsRepository _newsRepository;

    public GetNewssQueryHandler(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<Result<List<News>>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        var news = await _newsRepository.GetAsync(s => s.IsDeleted == false, false);
        return news.ToList();
    }
}