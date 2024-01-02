using MediatR;

namespace Application.Charts.Queries.GetInfoQuery;

public record GetInfoQuery(Guid Id) : IRequest<InfoModel>;