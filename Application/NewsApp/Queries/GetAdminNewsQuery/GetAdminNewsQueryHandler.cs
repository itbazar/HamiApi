using Application.Common.Interfaces.Persistence;
using Domain.Models.News;
using MediatR;

namespace Application.NewsApp.Queries.GetAdminNewsQuery;

internal class GetAdminNewssQueryHandler : 
    IRequestHandler<GetAdminNewsQuery, Result<List<News>>>
{
    private readonly INewsRepository _newsRepository;

    public GetAdminNewssQueryHandler(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<Result<List<News>>> Handle(GetAdminNewsQuery request, CancellationToken cancellationToken)
    {
        var news = await _newsRepository.GetAsync(null, false);
        return news.ToList();
    }
}