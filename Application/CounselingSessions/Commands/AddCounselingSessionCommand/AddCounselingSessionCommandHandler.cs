using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.CounselingSessions.Commands.AddCounselingSessionCommand;

internal class AddCounselingSessionCommandHandler(
    ICounselingSessionRepository counselingSessionRepository,
    IPatientGroupRepository patientGroupRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddCounselingSessionCommand, Result<CounselingSession>>
{
    public async Task<Result<CounselingSession>> Handle(AddCounselingSessionCommand request, CancellationToken cancellationToken)
    {
        var group = await patientGroupRepository.GetFirstAsync(q => q.Id == request.PatientGroupId);
        if (group is null)
            return UserErrors.UserGroupNotAssigned;
        var counselingSession = CounselingSession.Create(
            request.PatientGroupId,
            group.MentorId,
            request.ScheduledDate,
            request.Topic,
            request.MeetingLink
        ); 
        counselingSessionRepository.Insert(counselingSession);
        await unitOfWork.SaveAsync();
        return counselingSession;
    }
}