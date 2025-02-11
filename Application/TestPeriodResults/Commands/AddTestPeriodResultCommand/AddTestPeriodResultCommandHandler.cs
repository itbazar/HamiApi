using Application.Common.Interfaces.Persistence;
using Application.TestPeriodResults.Commands.AddTestPeriodResultCommand;
using Domain.Models.Hami;
using Microsoft.EntityFrameworkCore;

internal class AddTestPeriodResultCommandHandler(
    ITestPeriodResultRepository testPeriodResultRepository,
    ITestPeriodRepository testPeriodRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddTestPeriodResultCommand, Result<TestPeriodResult>>
{
    public async Task<Result<TestPeriodResult>> Handle(AddTestPeriodResultCommand request, CancellationToken cancellationToken)
    {
        // بررسی نوع تست
        return request.TestType == TestType.MOOD
            ? await HandleMoodTest(request, cancellationToken)
            : await HandleOtherTests(request, cancellationToken);
    }

    /// <summary>
    /// مدیریت ثبت مود روزانه
    /// </summary>
    private async Task<Result<TestPeriodResult>> HandleMoodTest(AddTestPeriodResultCommand request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        // چک کردن وجود مود روزانه برای امروز
        var existingMood = await unitOfWork.DbContext.Set<TestPeriodResult>()
            .FirstOrDefaultAsync(t => t.UserId == request.UserId &&
                                      t.TestType == TestType.MOOD &&
                                      t.CreatedAt.Date == today &&
                                      !t.IsDeleted, cancellationToken);

        if (existingMood is not null)
            return TestPeriodErrors.MoodExist;

        // گرفتن دوره آزمون
        var testPeriod = await GetTestPeriod(TestType.MOOD);
        if (testPeriod is null) return TestPeriodErrors.NotFound;

        // مقداردهی instance
        var testInstance = await GetNextTestInstance(request.UserId, testPeriod.Id);

        // ایجاد نتیجه تست
        var testPeriodResult = TestPeriodResult.Create(
            request.UserId,
            request.TestType,
            request.TotalScore,
            testPeriod.Id,
            testInstance
        );

        testPeriod.UpdateNextOccurrence();
        testPeriodResultRepository.Insert(testPeriodResult);
        await unitOfWork.SaveAsync();

        return testPeriodResult;
    }

    /// <summary>
    /// مدیریت ثبت سایر تست‌ها
    /// </summary>
    private async Task<Result<TestPeriodResult>> HandleOtherTests(AddTestPeriodResultCommand request, CancellationToken cancellationToken)
    {
        // گرفتن دوره آزمون
        var testPeriod = await GetTestPeriodById(request.TestPeriodId);
        if (testPeriod is null) return TestPeriodErrors.NotFound;

        // بررسی بازه زمانی
        if (!IsInValidDateRange(testPeriod))
        {
            return TestPeriodErrors.OutOfRangeDate; // خارج از بازه زمانی مجاز
        }

        // بررسی شرکت کاربر در این بازه
        if (await HasUserParticipatedInCurrentRecurrence(request.UserId, testPeriod))
        {
            return TestPeriodErrors.TestAlreadySubmitted;
        }

        // مقداردهی instance
        var testInstance = await GetNextTestInstance(request.UserId, testPeriod.Id);

        // ایجاد نتیجه تست
        var testPeriodResult = TestPeriodResult.Create(
            request.UserId,
            request.TestType,
            request.TotalScore,
            request.TestPeriodId,
            testInstance
        );

        testPeriod.UpdateNextOccurrence();
        testPeriodResultRepository.Insert(testPeriodResult);
        await unitOfWork.SaveAsync();

        return testPeriodResult;
    }

    /// <summary>
    /// گرفتن دوره آزمون براساس نوع
    /// </summary>
    private async Task<TestPeriod?> GetTestPeriod(TestType testType)
    {
        return await testPeriodRepository.GetSingleAsync(tp => tp.TestType == testType && !tp.IsDeleted);
    }

    /// <summary>
    /// گرفتن دوره آزمون براساس شناسه
    /// </summary>
    private async Task<TestPeriod?> GetTestPeriodById(Guid testPeriodId)
    {
        return await unitOfWork.DbContext.Set<TestPeriod>()
            .FirstOrDefaultAsync(tp => tp.Id == testPeriodId && !tp.IsDeleted);
    }

    /// <summary>
    /// بررسی بازه زمانی مجاز
    /// </summary>
    private bool IsInValidDateRange(TestPeriod testPeriod)
    {
        var now = DateTime.UtcNow;
        return testPeriod.Recurrence switch
        {
            RecurrenceType.None => now >= testPeriod.StartDate && now <= testPeriod.EndDate,
            RecurrenceType.Daily => now.Date >= testPeriod.StartDate.Date && now.Date <= testPeriod.EndDate.Date,
            RecurrenceType.Weekly => now <= testPeriod.EndDate,
            RecurrenceType.Monthly => now <= testPeriod.EndDate,
            _ => false
        };
    }

    /// <summary>
    /// بررسی شرکت در بازه فعلی
    /// </summary>
    /// 
    private async Task<bool> HasUserParticipatedInCurrentRecurrence(string userId, TestPeriod testPeriod)
    {
        var persianCalendar = new System.Globalization.PersianCalendar();

        // آخرین ثبت آزمون کاربر
        var lastParticipation = await unitOfWork.DbContext.Set<TestPeriodResult>()
            .Where(t => t.UserId == userId && t.TestPeriodId == testPeriod.Id && !t.IsDeleted)
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastParticipation is null) return false; // اگر کاربر هیچ ثبت نتیجه‌ای نداشته باشد

        var now = DateTime.UtcNow;

        // تاریخ شمسی فعلی
        var nowPersianYear = persianCalendar.GetYear(now);
        var nowPersianMonth = persianCalendar.GetMonth(now);
        var nowPersianDayOfWeek = (int)persianCalendar.GetDayOfWeek(now);

        // تاریخ شمسی آخرین شرکت
        var lastPersianYear = persianCalendar.GetYear(lastParticipation.CreatedAt);
        var lastPersianMonth = persianCalendar.GetMonth(lastParticipation.CreatedAt);
        var lastPersianDayOfWeek = (int)persianCalendar.GetDayOfWeek(lastParticipation.CreatedAt);

        // بررسی براساس نوع تکرار
        return testPeriod.Recurrence switch
        {
            RecurrenceType.None => true, // آزمون غیرتکرارشونده
            RecurrenceType.Daily => persianCalendar.GetDayOfYear(lastParticipation.CreatedAt) == persianCalendar.GetDayOfYear(now), // روزانه
            RecurrenceType.Weekly => nowPersianYear == lastPersianYear && // هفتگی شمسی
                                     Math.Abs(nowPersianDayOfWeek - lastPersianDayOfWeek) < 7,
            RecurrenceType.Monthly => nowPersianYear == lastPersianYear && // ماهانه شمسی
                                      nowPersianMonth == lastPersianMonth,
            _ => false
        };
    }

    //private async Task<bool> HasUserParticipatedInCurrentRecurrence(string userId, TestPeriod testPeriod)
    //{
    //    var lastParticipation = await unitOfWork.DbContext.Set<TestPeriodResult>()
    //        .Where(t => t.UserId == userId && t.TestPeriodId == testPeriod.Id && !t.IsDeleted)
    //        .OrderByDescending(t => t.CreatedAt)
    //        .FirstOrDefaultAsync();

    //    if (lastParticipation is null) return false;

    //    var now = DateTime.UtcNow;
    //    return testPeriod.Recurrence switch
    //    {
    //        RecurrenceType.None => true,
    //        RecurrenceType.Daily => lastParticipation.CreatedAt.Date == now.Date,
    //        RecurrenceType.Weekly => lastParticipation.CreatedAt > now.AddDays(-7),
    //        RecurrenceType.Monthly => lastParticipation.CreatedAt > now.AddMonths(-1),
    //        _ => false
    //    };
    //}

    /// <summary>
    /// گرفتن شماره تکرار بعدی
    /// </summary>
    private async Task<int> GetNextTestInstance(string userId, Guid testPeriodId)
    {
        var previousTestsCount = await unitOfWork.DbContext.Set<TestPeriodResult>()
            .CountAsync(t => t.TestPeriodId == testPeriodId &&
                             t.UserId == userId &&
                             !t.IsDeleted);

        return previousTestsCount + 1;
    }
}
