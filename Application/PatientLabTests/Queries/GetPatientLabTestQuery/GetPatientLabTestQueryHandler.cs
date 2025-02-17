using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.PatientLabTests.Queries.GetPatientLabTestQuery;

internal class GetPatientLabTestQueryHandler : IRequestHandler<GetPatientLabTestQuery, Result<PagedList<PatientLabTest>>>
{
    private readonly IPatientLabTestRepository _PatientLabTestRepository;

    public GetPatientLabTestQueryHandler(IPatientLabTestRepository PatientLabTestRepository)
    {
        _PatientLabTestRepository = PatientLabTestRepository;
    }

    public async Task<Result<PagedList<PatientLabTest>>> Handle(GetPatientLabTestQuery request, CancellationToken cancellationToken)
    {
        var PatientLabTest = await _PatientLabTestRepository.GetPagedAsync(
         request.PagingInfo,
         filter: s => !s.IsDeleted,
         trackChanges: false,
         orderBy: s => s.OrderByDescending(o => o.CreatedAt),
         includeProperties: "User" // بارگذاری User
         );

        return PatientLabTest;
    }

}