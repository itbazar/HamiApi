using Domain.Models.Hami;
using MediatR;

namespace Application.CounselingSessionApp.Queries.GetCounselingSessionByIdQuery;

public record GetCounselingSessionByIdQuery(Guid Id) : IRequest<Result<CounselingSession>>;
