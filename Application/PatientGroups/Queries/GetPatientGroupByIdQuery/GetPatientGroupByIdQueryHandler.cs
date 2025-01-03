using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.PatientGroupApp.Queries.GetPatientGroupByIdQuery;

internal class GetPatientGroupByIdQueryHandler : IRequestHandler<GetPatientGroupByIdQuery, Result<PatientGroup>>
{
    private readonly IPatientGroupRepository _patientGroupRepository;

    public GetPatientGroupByIdQueryHandler(IPatientGroupRepository patientGroupRepository)
    {
        _patientGroupRepository = patientGroupRepository;
    }

    public async Task<Result<PatientGroup>> Handle(GetPatientGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var patientGroup = await _patientGroupRepository.GetSingleAsync(s => s.Id == request.Id && s.IsDeleted != true, false);
        if (patientGroup is null)
            return GenericErrors.NotFound;
        return patientGroup;
    }
}