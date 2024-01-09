using Application.Common.Interfaces.Persistence;
using Domain.Models.News;
using MediatR;

namespace Application.NewsApp.Queries.GetNewsByIdQuery;

internal class GetNewsByIdQueryHandler : IRequestHandler<GetNewsByIdQuery, News>
{
    private readonly INewsRepository _newsRepository;

    public GetNewsByIdQueryHandler(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<News> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
    {
        var news = await _newsRepository.GetSingleAsync(s => s.Id == request.Id, false);
        if (news is null)
            throw new Exception("Not found.");
        return news;
    }
}