using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Application.Users.Common;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetPatientTestPeriodsReport;

internal class GetPatientTestPeriodsReportQueryHandler(IUserRepository userRepository, IUnitOfWork unitOfWork,
    IPatientGroupRepository patientGroupRepository, ITestPeriodRepository TestPeriodRepository) : IRequestHandler<GetPatientTestPeriodsReportQuery, Result<List<TestPeriodResponse>>>
{
    public async Task<Result<List<TestPeriodResponse>>> Handle(GetPatientTestPeriodsReportQuery request, CancellationToken cancellationToken)
    {
        //var patientFirst = await unitOfWork.DbContext.Set<UserGroupMembership>()
        //    .Where(r => r.UserId == request.UserId && !r.IsDeleted).FirstOrDefaultAsync();

        //if (patientFirst is null)
        //    return UserErrors.UserGroupNotAssigned; // اگر گروهی وجود ندارد

        // گرفتن جلسات گروه
        var testPeriods = await unitOfWork.DbContext.Set<TestPeriod>()
            .Where(tp => !tp.IsDeleted &&
                tp.TestType != TestType.MOOD &&
                tp.Code != 101 &&
                tp.Code != 102 &&
                tp.StartDate <= DateTime.UtcNow &&
                tp.EndDate >= DateTime.UtcNow
            )
            .OrderBy(tp => tp.StartDate)
            .ToListAsync(cancellationToken);

        // وضعیت شرکت در آزمون‌ها
        var userTestResults = await unitOfWork.DbContext.Set<TestPeriodResult>()
            .Where(tr => tr.UserId == request.UserId && !tr.IsDeleted)
            .ToListAsync(cancellationToken);

        // مشخص کردن وضعیت شرکت کاربر در آزمون‌ها
        var testWithParticipationStatus = testPeriods.Select(tp => new TestPeriodResponse(
    tp.Id,
    tp.TestType,
    tp.PeriodName,
    tp.StartDate,
    tp.EndDate,
    tp.Code,
    userTestResults.Any(utr => utr.TestPeriodId == tp.Id)
)).ToList();


        return testWithParticipationStatus;
    }
}
