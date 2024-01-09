﻿using Application.Common.Interfaces.Persistence;
using Domain.Models.News;
using MediatR;

namespace Application.NewsApp.Queries.GetNewsQuery;

internal class GetNewssQueryHandler : IRequestHandler<GetNewssQuery, List<News>>
{
    private readonly INewsRepository _newsRepository;

    public GetNewssQueryHandler(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<List<News>> Handle(GetNewssQuery request, CancellationToken cancellationToken)
    {
        var newss = await _newsRepository.GetAsync(s => s.IsDeleted == false, false);
        return newss.ToList();
    }
}