using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.CounselingSessions.Commands.AddCounselingSessionCommand;

internal class AddCounselingSessionCommandHandler(
    ICounselingSessionRepository counselingSessionRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddCounselingSessionCommand, Result<CounselingSession>>
{
    public async Task<Result<CounselingSession>> Handle(AddCounselingSessionCommand request, CancellationToken cancellationToken)
    {
        var counselingSession = CounselingSession.Create(
            request.PatientGroupId,
            request.MentorId,
            request.ScheduledDate,
            request.Topic,
            request.MeetingLink
        ); 
        counselingSessionRepository.Insert(counselingSession);
        await unitOfWork.SaveAsync();
        return counselingSession;
    }
}