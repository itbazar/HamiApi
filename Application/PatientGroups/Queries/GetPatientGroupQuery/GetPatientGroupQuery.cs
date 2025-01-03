using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.PatientGroups.Queries.GetPatientGroupQuery;

public record GetPatientGroupQuery(PagingInfo PagingInfo) : IRequest<Result<PagedList<PatientGroup>>>;
