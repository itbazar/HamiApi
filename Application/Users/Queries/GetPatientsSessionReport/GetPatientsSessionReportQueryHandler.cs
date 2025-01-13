using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetPatientsSessionReport;

internal class GetPatientsSessionReportQueryHandler(IUserRepository userRepository,IUnitOfWork unitOfWork,
    IPatientGroupRepository patientGroupRepository, ICounselingSessionRepository counselingSessionRepository) : IRequestHandler<GetPatientsSessionReportQuery, Result<List<CounselingSession>>>
{
    public async Task<Result<List<CounselingSession>>> Handle(GetPatientsSessionReportQuery request, CancellationToken cancellationToken)
    {
        var patientFirst = await unitOfWork.DbContext.Set<UserGroupMembership>()
            .Where(r => r.UserId == request.UserId && !r.IsDeleted).FirstOrDefaultAsync();

        if (patientFirst is null)
            return UserErrors.UserGroupNotAssigned; // اگر گروهی وجود ندارد


        // گرفتن جلسات گروه
        var sessions = await counselingSessionRepository.GetAsync(cs =>
            cs.PatientGroupId == patientFirst.PatientGroupId &&
            !cs.IsDeleted
            //&& cs.ScheduledDate <= DateTime.UtcNow
            , includeProperties: "Mentor,PatientGroup,AttendanceLogs");

        return sessions.ToList();
    }
}
