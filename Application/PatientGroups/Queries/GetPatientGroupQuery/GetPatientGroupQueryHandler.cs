using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.PatientGroups.Queries.GetPatientGroupQuery;

internal class GetPatientGroupQueryHandler : IRequestHandler<GetPatientGroupQuery, Result<PagedList<PatientGroup>>>
{
    private readonly IPatientGroupRepository _patientGroupRepository;

    public GetPatientGroupQueryHandler(IPatientGroupRepository patientGroupRepository)
    {
        _patientGroupRepository = patientGroupRepository;
    }

    public async Task<Result<PagedList<PatientGroup>>> Handle(GetPatientGroupQuery request, CancellationToken cancellationToken)
    {
        var patientGroup = await _patientGroupRepository.GetPagedAsync(
            request.PagingInfo,
            s => s.IsDeleted == false,
            false,
            s => s.OrderByDescending(o => o.Stage));
        return patientGroup;
    }
}