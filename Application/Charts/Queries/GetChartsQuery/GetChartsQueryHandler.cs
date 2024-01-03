﻿using Application.Common.Interfaces.Persistence;
using Domain.Models.ChartAggregate;
using MediatR;

namespace Application.Charts.Queries.GetChartsQuery;

internal class GetChartsQueryHandler : IRequestHandler<GetChartsQuery, List<Chart>>
{
    private readonly IChartRepository _chartRepository;

    public GetChartsQueryHandler(IChartRepository chartRepository)
    {
        _chartRepository = chartRepository;
    }

    public async Task<List<Chart>> Handle(GetChartsQuery request, CancellationToken cancellationToken)
    {
        var result = await _chartRepository.GetAsync(
            cc => cc.IsDeleted == false && request.RoleNames.Any(rn => cc.Roles.Any(cr => cr.Name == rn)),
            false,
            cc => cc.OrderBy(o => o.Order));

        return result.ToList();
    }
}
