using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.PatientLabTests.Queries.GetPatientLabTestByIdQuery;

internal class GetPatientLabTestByIdQueryHandler : IRequestHandler<GetPatientLabTestByIdQuery, Result<PatientLabTest>>
{
    private readonly IPatientLabTestRepository _PatientLabTestRepository;

    public GetPatientLabTestByIdQueryHandler(IPatientLabTestRepository PatientLabTestRepository)
    {
        _PatientLabTestRepository = PatientLabTestRepository;
    }

    public async Task<Result<PatientLabTest>> Handle(GetPatientLabTestByIdQuery request, CancellationToken cancellationToken)
    {
        var PatientLabTest = await _PatientLabTestRepository.GetSingleAsync(s => s.Id == request.Id && s.IsDeleted != true, false);
        if (PatientLabTest is null)
            return GenericErrors.NotFound;
        return PatientLabTest;
    }
}