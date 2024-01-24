using Application.Common.Interfaces.Persistence;
using Domain.Models.News;

namespace Application.NewsApp.Queries.GetAdminNewsByIdQuery;

internal class GetAdminNewsByIdQueryHandler : IRequestHandler<GetAdminNewsByIdQuery, Result<News>>
{
    private readonly INewsRepository _newsRepository;

    public GetAdminNewsByIdQueryHandler(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<Result<News>> Handle(GetAdminNewsByIdQuery request, CancellationToken cancellationToken)
    {
        var news = await _newsRepository.GetSingleAsync(s => s.Id == request.Id, false);
        if (news is null)
            return GenericErrors.NotFound;
        return news;
    }
}