using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;
using Microsoft.EntityFrameworkCore;

namespace Application.TestPeriodResults.Commands.AddTestPeriodResultCommand;

internal class AddTestPeriodResultCommandHandler(
    ITestPeriodResultRepository testPeriodResultRepository,
    ITestPeriodRepository testPeriodRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddTestPeriodResultCommand, Result<TestPeriodResult>>
{
    public async Task<Result<TestPeriodResult>> Handle(AddTestPeriodResultCommand request, CancellationToken cancellationToken)
    {
        // بررسی نوع تست
        return request.TestType == TestType.MOOD
            ? await HandleMoodTest(request)
            : await HandleOtherTests(request);
    }

    /// <summary>
    /// مدیریت ثبت مود روزانه
    /// </summary>
    private async Task<Result<TestPeriodResult>> HandleMoodTest(AddTestPeriodResultCommand request)
    {
        var today = DateTime.UtcNow.Date;

        // چک کردن وجود مود روزانه برای امروز
        var existingMood = await unitOfWork.DbContext.Set<TestPeriodResult>()
            .FirstOrDefaultAsync(t => t.UserId == request.UserId &&
                                      t.TestType == TestType.MOOD &&
                                      t.CreatedAt.Date == today &&
                                      !t.IsDeleted);

        if (existingMood is not null)
            return TestPeriodErrors.MoodExist;

        var testPeriod = await testPeriodRepository
            .GetSingleAsync(tp => tp.TestType == TestType.MOOD && !tp.IsDeleted);

        if (testPeriod == null || testPeriod.Id == Guid.Empty)
            return TestPeriodErrors.NotFound;

        var testPeriodResult = TestPeriodResult.Create(
            request.UserId,
            request.TestType,
            request.TotalScore,
            testPeriod.Id
        );

        testPeriodResultRepository.Insert(testPeriodResult);
        await unitOfWork.SaveAsync();

        return testPeriodResult;
    }

    /// <summary>
    /// مدیریت ثبت سایر تست‌ها
    /// </summary>
    private async Task<Result<TestPeriodResult>> HandleOtherTests(AddTestPeriodResultCommand request)
    {
        // گرفتن اطلاعات دوره آزمون از دیتابیس
        var testPeriod = await unitOfWork.DbContext.Set<TestPeriod>()
            .FirstOrDefaultAsync(tp => tp.Id == request.TestPeriodId && !tp.IsDeleted);

        // اگر دوره آزمون پیدا نشد
        if (testPeriod is null)
        {
            return TestPeriodErrors.NotFound; // خطا: دوره آزمون پیدا نشد
        }

        // چک کردن اینکه آیا در محدوده تاریخ مجاز دوره هستیم
        var now = DateTime.UtcNow;
        if (now < testPeriod.StartDate || now > testPeriod.EndDate)
        {
            return TestPeriodErrors.OutOfRangeDate; // خطا: خارج از محدوده زمانی مجاز
        }

        // بررسی اینکه آیا کاربر در این دوره قبلاً آزمون ثبت کرده است
        var existingResult = await unitOfWork.DbContext.Set<TestPeriodResult>()
            .FirstOrDefaultAsync(t => t.UserId == request.UserId &&
                                      t.TestPeriodId == request.TestPeriodId &&
                                      !t.IsDeleted);

        if (existingResult is not null)
        {
            // برگرداندن خطا اگر قبلاً نتیجه ثبت شده باشد
            return TestPeriodErrors.TestAlreadySubmitted;
        }

        // ایجاد نتیجه تست برای سایر تست‌ها
        var testPeriodResult = TestPeriodResult.Create(
            request.UserId,
            request.TestType,
            request.TotalScore,
            request.TestPeriodId
        );

        // درج نتیجه جدید در دیتابیس
        testPeriodResultRepository.Insert(testPeriodResult);

        // ذخیره تغییرات
        await unitOfWork.SaveAsync();

        return testPeriodResult;
    }
}
