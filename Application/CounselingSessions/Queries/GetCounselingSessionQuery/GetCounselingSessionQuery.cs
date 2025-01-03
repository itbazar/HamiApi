using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.CounselingSessions.Queries.GetCounselingSessionQuery;

public record GetCounselingSessionQuery(PagingInfo PagingInfo) : IRequest<Result<PagedList<CounselingSession>>>;
