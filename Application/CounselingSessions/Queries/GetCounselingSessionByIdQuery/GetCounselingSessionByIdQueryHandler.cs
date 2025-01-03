using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.CounselingSessionApp.Queries.GetCounselingSessionByIdQuery;

internal class GetCounselingSessionByIdQueryHandler : IRequestHandler<GetCounselingSessionByIdQuery, Result<CounselingSession>>
{
    private readonly ICounselingSessionRepository _counselingSessionRepository;

    public GetCounselingSessionByIdQueryHandler(ICounselingSessionRepository counselingSessionRepository)
    {
        _counselingSessionRepository = counselingSessionRepository;
    }

    public async Task<Result<CounselingSession>> Handle(GetCounselingSessionByIdQuery request, CancellationToken cancellationToken)
    {
        var counselingSession = await _counselingSessionRepository.GetSingleAsync(s => s.Id == request.Id && s.IsDeleted != true, false);
        if (counselingSession is null)
            return GenericErrors.NotFound;
        return counselingSession;
    }
}