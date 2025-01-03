using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.Hami;
using Infrastructure.Storage;

namespace Application.CounselingSessions.Commands.UpdateCounselingSessionCommand;

internal class UpdateCounselingSessionCommandHandler(
    ICounselingSessionRepository counselingSessionRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateCounselingSessionCommand, Result<CounselingSession>>
{
    public async Task<Result<CounselingSession>> Handle(UpdateCounselingSessionCommand request, CancellationToken cancellationToken)
    {
        var counselingSession = await counselingSessionRepository.GetSingleAsync(s => s.Id == request.Id);
        if (counselingSession is null)
            throw new Exception("Not found.");
        counselingSession.Update(request.ScheduledDate, request.Topic, request.MeetingLink, request.MentorNote);
        counselingSessionRepository.Update(counselingSession);
        await unitOfWork.SaveAsync();
        return counselingSession;
    }
}