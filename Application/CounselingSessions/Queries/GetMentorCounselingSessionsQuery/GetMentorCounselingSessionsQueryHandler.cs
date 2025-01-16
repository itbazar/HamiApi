using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.CounselingSessions.Queries.GetMentorCounselingSessionsQuery;

internal class GetMentorCounselingSessionsQueryHandler(ICounselingSessionRepository patientGroupRepository) : IRequestHandler<GetMentorCounselingSessionsQuery, Result<List<CounselingSession>>>
{
    public async Task<Result<List<CounselingSession>>> Handle(GetMentorCounselingSessionsQuery request, CancellationToken cancellationToken)
    {
        var sessions = await patientGroupRepository.GetAsync(
       filter: q => q.MentorId == request.MentorId &&
                    q.PatientGroupId == request.PatientGroupId &&
                    !q.IsDeleted,
       orderBy: q => q.OrderByDescending(s => s.ScheduledDate), // مرتب‌سازی نزولی بر اساس ScheduledDate
       includeProperties: "Mentor,PatientGroup"
   );
        return sessions.ToList();
    }
}