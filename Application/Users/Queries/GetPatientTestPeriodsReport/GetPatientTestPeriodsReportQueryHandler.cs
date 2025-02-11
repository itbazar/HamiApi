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
        // گرفتن آزمون‌هایی که کاربر می‌تواند شرکت کند
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

        // گرفتن نتایج آزمون‌های کاربر
        var userTestResults = await unitOfWork.DbContext.Set<TestPeriodResult>()
            .Where(tr => tr.UserId == request.UserId && !tr.IsDeleted)
            .ToListAsync(cancellationToken);

        // مشخص کردن وضعیت شرکت کاربر در آزمون‌ها
        var testWithParticipationStatus = testPeriods.Select(tp =>
        {
            // گرفتن آخرین نتیجه کاربر برای این آزمون
            var lastResult = userTestResults
                .Where(utr => utr.TestPeriodId == tp.Id)
                .OrderByDescending(utr => utr.CreatedAt)
                .FirstOrDefault();

            // بررسی بازه زمانی براساس RecurrenceType
            var canParticipate = lastResult == null || IsAllowedToRetake(tp, lastResult.CreatedAt);

            return new TestPeriodResponse(
                tp.Id,
                tp.TestType,
                tp.PeriodName,
                tp.StartDate,
                tp.EndDate,
                tp.Code,
                !canParticipate // اگر نمی‌تواند شرکت کند، یعنی شرکت کرده است
            );
        }).ToList();

        return testWithParticipationStatus;
    }

    private bool IsAllowedToRetake(TestPeriod testPeriod, DateTime lastParticipationDate)
    {
        if (testPeriod.Recurrence == RecurrenceType.None)
            return false; // آزمون تکراری نیست، کاربر نمی‌تواند دوباره شرکت کند

        var now = DateTime.UtcNow;

        return testPeriod.Recurrence switch
        {
            RecurrenceType.Daily => lastParticipationDate.Date < now.Date, // اگر تاریخ آخرین شرکت قبل از امروز است
            RecurrenceType.Weekly => lastParticipationDate.Date < now.AddDays(-7).Date, // اگر بیش از یک هفته گذشته باشد
            RecurrenceType.Monthly => lastParticipationDate.Date < now.AddMonths(-1).Date, // اگر بیش از یک ماه گذشته باشد
            _ => false
        };
    }


}
