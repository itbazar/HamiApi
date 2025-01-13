using MediatR;

namespace Application.Charts.Queries.GetInfoQuery;

public record GetInfoQuery(int Code,string userId="",string userRole="") : IRequest<Result<InfoModel>>;