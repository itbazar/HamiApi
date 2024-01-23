using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;

namespace Application.Complaints.Queries.GetPossibleStatesCountQuery;

public record GetPossibleStatesCountQuery() : IRequest<Result<List<GetPossibleStatesCountResponse>>>;
public record GetPossibleStatesCountResponse(string Title, int Value, int Count);