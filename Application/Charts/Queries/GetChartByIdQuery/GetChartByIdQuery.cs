using Domain.Models.ChartAggregate;
using MediatR;

namespace Application.Charts.Queries.GetChartByIdQuery;

public record GetChartByIdQuery(Guid Id) : IRequest<Result<ChartResponse>>;

public record ChartResponse(
    Guid Id,
    int Order,
    int Code,
    string Title,
    string Parameters,
    long ValidForMilliseconds,
    bool IsDeleted);