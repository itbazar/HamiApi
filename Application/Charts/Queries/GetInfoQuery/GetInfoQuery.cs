using MediatR;

namespace Application.Charts.Queries.GetInfoQuery;

public record GetInfoQuery(int Code) : IRequest<Result<InfoModel>>;