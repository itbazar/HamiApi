using Application.Common.Interfaces.Persistence;
using Domain.Models.News;
using MediatR;

namespace Application.NewsApp.Queries.GetAdminNewsQuery;

internal class GetAdminNewssQueryHandler : IRequestHandler<GetAdminNewsQuery, List<News>>
{
    private readonly INewsRepository _newsRepository;

    public GetAdminNewssQueryHandler(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<List<News>> Handle(GetAdminNewsQuery request, CancellationToken cancellationToken)
    {
        var newss = await _newsRepository.GetAsync(null, false);
        return newss.ToList();
    }
}