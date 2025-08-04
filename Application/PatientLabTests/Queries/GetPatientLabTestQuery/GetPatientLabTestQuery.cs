using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.PatientLabTests.Queries.GetPatientLabTestQuery;

public record GetPatientLabTestQuery(PagingInfo PagingInfo,string UserId="") : IRequest<Result<PagedList<PatientLabTest>>>;
