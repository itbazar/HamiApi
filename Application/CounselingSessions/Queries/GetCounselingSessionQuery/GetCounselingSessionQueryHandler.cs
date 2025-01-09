using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.CounselingSessions.Queries.GetCounselingSessionQuery;

internal class GetCounselingSessionQueryHandler : IRequestHandler<GetCounselingSessionQuery, Result<PagedList<CounselingSession>>>
{
    private readonly ICounselingSessionRepository _counselingSessionRepository;

    public GetCounselingSessionQueryHandler(ICounselingSessionRepository counselingSessionRepository)
    {
        _counselingSessionRepository = counselingSessionRepository;
    }

    public async Task<Result<PagedList<CounselingSession>>> Handle(GetCounselingSessionQuery request, CancellationToken cancellationToken)
    {
        var counselingSession = await _counselingSessionRepository.GetPagedAsync(
            request.PagingInfo,
            s => s.IsDeleted == false,
            false,
            s => s.OrderByDescending(o => o.ScheduledDate),
            includeProperties: "Mentor,PatientGroup" // بارگذاری Mentor 
         );
        return counselingSession;
    }
}