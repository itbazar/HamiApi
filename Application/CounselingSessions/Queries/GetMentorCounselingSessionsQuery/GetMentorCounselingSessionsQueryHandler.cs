using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.CounselingSessions.Queries.GetMentorCounselingSessionsQuery;

internal class GetMentorCounselingSessionsQueryHandler(ICounselingSessionRepository patientGroupRepository) : IRequestHandler<GetMentorCounselingSessionsQuery, Result<List<CounselingSession>>>
{
    public async Task<Result<List<CounselingSession>>> Handle(GetMentorCounselingSessionsQuery request, CancellationToken cancellationToken)
    {
        var groups = await patientGroupRepository
            .GetAsync(q => q.MentorId == request.MentorId && q.PatientGroupId == request.PatientGroupId && !q.IsDeleted,
            includeProperties: "Mentor,PatientGroup");
        return groups.ToList();
    }
}