using Application.Common.Interfaces.Persistence;
using Domain.Models.News;

namespace Application.NewsApp.Queries.GetNewsByIdQuery;

internal class GetNewsByIdQueryHandler : IRequestHandler<GetNewsByIdQuery, Result<News>>
{
    private readonly INewsRepository _newsRepository;

    public GetNewsByIdQueryHandler(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<Result<News>> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
    {
        var news = await _newsRepository.GetSingleAsync(s => s.Id == request.Id && s.IsDeleted != true, false);
        if (news is null)
            return GenericErrors.NotFound;
        return news;
    }
}