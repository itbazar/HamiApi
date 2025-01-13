using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using Domain.Models.IdentityAggregate;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Statics;
using System.Globalization;
using System.Linq.Expressions;
using SharedKernel.ExtensionMethods;
using Domain.Models.Hami;
using System.Security.Claims;

namespace Application.Charts.Queries.GetInfoQuery;

internal class GetInfoQueryHandler(IUnitOfWork unitOfWork, IUserRepository userRepository) : IRequestHandler<GetInfoQuery, Result<InfoModel>>
{
    public async Task<Result<InfoModel>> Handle(GetInfoQuery request, CancellationToken cancellationToken)
    {
        InfoModel result = new InfoModel();
        switch (request.Code)
        {
            case ChartCodes.Dashboard:
                result = await GetGeneralReport(request.userId,request.userRole);
                break;
            case ChartCodes.ComplaintCategoryHistogram:
                var bins = await unitOfWork.DbContext.Set<ComplaintCategory>().Select(cc => new Bin<Guid>(cc.Id, cc.Title)).ToListAsync();
                result = await GetHistogram(
                    "فراوانی دسته بندی ها",
                    bins,
                    unitOfWork.DbContext.Set<Complaint>(),
                    c => c.CategoryId); 
                break;
            case ChartCodes.ComplaintOrganizationHistogram:
                var bins3 = await unitOfWork.DbContext.Set<ComplaintOrganization>().Select(co => new Bin<Guid?>(co.Id, co.Title)).ToListAsync();
                result = await GetHistogram(
                    "فراوانی سازمان ها",
                    bins3,
                    unitOfWork.DbContext.Set<Complaint>(),
                    c => c.ComplaintOrganizationId);
                break;
            case ChartCodes.ComplaintStatusHistogram:
                var bins2 = GetBins<ComplaintState>();
                result = await GetHistogram(
                    "فراوانی وضعیت ها",
                    bins2,
                    unitOfWork.DbContext.Set<Complaint>(),
                    c => c.Status);
                break;
            case ChartCodes.PatientMentalScores:
                result = await GetPatientMentalReportByUserId(request.userId);
                break;

            case ChartCodes.PatientDailyMood:
                result = await GetPatientDailyMoodReportByUserId(request.userId);
                break;
            default:
                throw new Exception();
        }

        return result;
    }    

    //private async Task<InfoModel> GetHistogram<T, T2>(
    //    DbSet<T> valuesDbSet,
    //    Func<T, string> title,
    //    IQueryable<T2> query,
    //    Expression<Func<T2, Guid>> groupBy) where T : Entity
    //{
    //    var values = await valuesDbSet.ToListAsync();
    //    var hist = await query.GroupBy(groupBy)
    //        .Select(grp => new { Id = grp.Key, Count = grp.Count() })
    //        .ToListAsync();

    //    var result = new InfoModel();
    //    var serie = new InfoSerie("", "");
    //    result.Add(new InfoChart("Title", "", false, false).Add(serie));


    //    foreach (var value in values)
    //    {
    //        var item = hist.Where(h => h.Id == value.Id).FirstOrDefault();
    //        if (item is null)
    //        {
    //            serie.Add(title.Invoke(value), "0", "0");
    //        }
    //        else
    //        {
    //            serie.Add(title.Invoke(value), item.Count.ToString(), item.Count.ToString());
    //        }
    //    }
    //    return result;
    //}

    private async Task<InfoModel> GetHistogram<T, Key>(
        string title,
        List<Bin<Key>> bins,
        IQueryable<T> query,
        Expression<Func<T, Key>> groupBy)
    {
        var hist = await query.GroupBy(groupBy)
            .Select(grp => new { Id = grp.Key, Count = grp.Count() })
            .ToListAsync();

        var result = new InfoModel();
        var serie = new InfoSerie("فراوانی", "");
        result.Add(new InfoChart(title, "", false, false).Add(serie));


        foreach (var bin in bins)
        {
            var item = hist.Where(h => EqualityComparer<Key>.Default.Equals(h.Id, bin.Id)).FirstOrDefault();
            if (item is null)
            {
                serie.Add(bin.Title, "0", "0");
            }
            else
            {
                serie.Add(bin.Title, item.Count.ToString(), item.Count.ToString());
            }
        }
        return result;
    }
    private record Bin<Key>(Key Id, string Title);
    private static List<Bin<T>> GetBins<T>() where T:Enum
    { 
        var values = (T[])Enum.GetValues(typeof(T)); 
        var bins = new List<Bin<T>>();
        values.ToList().ForEach(bin => { bins.Add(new Bin<T>(bin, bin.GetDescription() ?? "")); });
        return bins;
    }

    private async Task<InfoModel> GetSummary()
    {
        var result = new InfoModel();

        var anonymousComplaintsCount = await unitOfWork.DbContext.Set<Complaint>()
            .Where(c => c.UserId == null)
            .CountAsync();
        result.Add(new InfoSingleton(anonymousComplaintsCount.ToString(), "گزارش ناشناس", ""));

        var knownComplaintsCount = await unitOfWork.DbContext.Set<Complaint>()
            .Where(c => c.UserId != null)
            .CountAsync();
        result.Add(new InfoSingleton(knownComplaintsCount.ToString(), "گزارش شناس", ""));

        var totalComplaintsCount = anonymousComplaintsCount + knownComplaintsCount;
        result.Add(new InfoSingleton(totalComplaintsCount.ToString(), "گزارش", ""));

        var totalUsers = await unitOfWork.DbContext.Set<ApplicationUser>().CountAsync();
        result.Add(new InfoSingleton(totalUsers.ToString(), "کاربر", ""));

        var now = DateTime.UtcNow;
        var query = unitOfWork.DbContext.Set<Complaint>().Where(c => true);
        var requestPerDay = await query.Where(p => p.RegisteredAt >= now.AddDays(-30))
                .GroupBy(q => q.RegisteredAt.DayOfYear)
                .Select(r => new { DayOfYear = r.Key, Count = r.Count() })
                .ToListAsync();
        requestPerDay.Sort((a, b) => a.DayOfYear.CompareTo(b.DayOfYear));
        var rpd = requestPerDay.Select(p => new { Date = new DateTime(now.Year, 1, 1).AddDays(p.DayOfYear - 1), Count = p.Count }).ToList();
        DateTime date;
        string dateStr;
        var pc = new PersianCalendar();
        var prpd = new List<DataItem>();

        for (int i = -30; i < 0; i++)
        {
            date = new DateTime(now.Year, 1, 1).AddDays(now.DayOfYear + i);
            dateStr = $"{pc.GetYear(date)}/{pc.GetMonth(date)}/{pc.GetDayOfMonth(date)}";
            var t = rpd.Where(a => a.Date == date).SingleOrDefault();
            if (t != null)
            {
                prpd.Add(new DataItem(dateStr, t.Count.ToString(), t.Count.ToString(), null));
            }
            else
            {
                prpd.Add(new DataItem(dateStr, "0", "0", null));
            }
        }
        result.Add(new InfoChart("تعداد گزارش ثبت شده در سی روز گذشته", "", false, false).Add(new InfoSerie("گزارش", "").Add(prpd)));


        return result;
    }
    private async Task<InfoModel> GetPatientMentalReportByUserId(string userId)
    {
        var result = new InfoModel();

        // تعداد تست‌های GAD
        var gadCount = await unitOfWork.DbContext.Set<TestPeriodResult>()
            .CountAsync(r => r.UserId == userId && r.TestType == TestType.GAD && !r.IsDeleted);

        result.Add(new InfoSingleton(gadCount.ToString(), "تعداد تست GAD", ""));

        // تعداد تست‌های MDD
        var mddCount = await unitOfWork.DbContext.Set<TestPeriodResult>()
            .CountAsync(r => r.UserId == userId && r.TestType == TestType.MDD && !r.IsDeleted);

        result.Add(new InfoSingleton(mddCount.ToString(), "تعداد تست MDD", ""));

        // گرفتن شناسه گروه کاربر
        var patientGroupId = await unitOfWork.DbContext.Set<UserGroupMembership>()
            .Where(u => u.UserId == userId && !u.IsDeleted)
            .Select(u => u.PatientGroupId)
            .FirstOrDefaultAsync();

        var patient = await unitOfWork.DbContext.Set<ApplicationUser>()
            .Where(u => u.Id == userId)
            .Select(u => new { u.FirstName, u.LastName,u.UserName })
            .SingleOrDefaultAsync();
        string patientFullName = patient != null ? $"{patient.FirstName} {patient.LastName} - {patient.UserName}" : "نامشخص";

        if (patientGroupId == Guid.Empty)
        {
            result.Add(new InfoSingleton("0", $"تعداد کل جلسات گروه بیمار", ""));
            result.Add(new InfoSingleton("0", $"تعداد جلساتی که بیمار ({patientFullName}) در آنها حاضر بوده", ""));
            return result;
        }

        // تعداد کل جلسات تعریف شده برای گروه کاربر
        var totalSessionsForGroup = await unitOfWork.DbContext.Set<CounselingSession>()
            .CountAsync(s => s.PatientGroupId == patientGroupId && !s.IsDeleted);

        result.Add(new InfoSingleton(totalSessionsForGroup.ToString(), $"تعداد کل جلسات گروه بیمار", ""));

        // تعداد جلساتی که کاربر در آن‌ها حاضر بوده
        var sessionsAttendedByUser = await unitOfWork.DbContext.Set<SessionAttendanceLog>()
            .CountAsync(l => l.UserId == userId && l.Attended);

        result.Add(new InfoSingleton(sessionsAttendedByUser.ToString(), $"تعداد جلساتی که بیمار ({patientFullName}) در آنها حاضر بوده", ""));

        // گرفتن 20 نتیجه آخر کاربر برای GAD و MDD
        var results = await unitOfWork.DbContext.Set<TestPeriodResult>()
            .Where(r => r.UserId == userId && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .Take(20)
            .ToListAsync();

        // دسته‌بندی نتایج برای GAD
        var gadResults = results
            .Where(r => r.TestType == TestType.GAD)
            .OrderBy(r => r.CreatedAt) // مرتب‌سازی صعودی برای نمودار
            .Select(r => new DataItem(
                r.CreatedAt.ToString("yyyy/MM/dd"), // تاریخ در فرمت مورد نظر
                r.TotalScore.ToString(),
                GetGadScoreDescription(r.TotalScore), // افزودن توضیح کیفی GAD
                null))
            .ToList();

        // دسته‌بندی نتایج برای MDD
        var mddResults = results
            .Where(r => r.TestType == TestType.MDD)
            .OrderBy(r => r.CreatedAt) // مرتب‌سازی صعودی برای نمودار
            .Select(r => new DataItem(
                r.CreatedAt.ToString("yyyy/MM/dd"), // تاریخ در فرمت مورد نظر
                r.TotalScore.ToString(),
                GetMddScoreDescription(r.TotalScore), // افزودن توضیح کیفی MDD
                null))
            .ToList();

        // افزودن نمودار GAD و MDD
        result.Add(
            new InfoChart("نتایج تست‌های روانی", "", false, false)
                .Add(new InfoSerie("GAD", "").Add(gadResults))
                .Add(new InfoSerie("MDD", "").Add(mddResults))
        );

        return result;
    }

    private async Task<InfoModel> GetGeneralReport(string userId, string userRole)
    {
        var result = new InfoModel();

        if (userRole == "Admin")
        {
            // گزارش برای ادمین

            // تعداد کل بیماران
            var totalPatients = await userRepository.GetPagedPatientsAsync(
                new PagingInfo { PageNumber = 1, PageSize = int.MaxValue }, null, "");
            result.Add(new InfoSingleton(totalPatients.Count.ToString(), "تعداد کل بیماران", ""));

            // تعداد کل منتورها
            var totalMentors = await userRepository.GetPagedMentorsAsync(
                new PagingInfo { PageNumber = 1, PageSize = int.MaxValue }, null);
            result.Add(new InfoSingleton(totalMentors.Count.ToString(), "تعداد کل منتورها", ""));

            // تعداد کل گروه‌های درمانی
            var totalGroups = await unitOfWork.DbContext.Set<PatientGroup>()
                .CountAsync(g => !g.IsDeleted);

            result.Add(new InfoSingleton(totalGroups.ToString(), "تعداد کل گروه‌های درمانی", ""));

            // تعداد کل جلسات
            var totalSessions = await unitOfWork.DbContext.Set<CounselingSession>()
                .CountAsync(s => !s.IsDeleted);

            result.Add(new InfoSingleton(totalSessions.ToString(), "تعداد کل جلسات", ""));

            // اضافه کردن نمودار میانگین مود روزانه برای ادمین
            var oneMonthAgo = DateTime.UtcNow.AddMonths(-1).Date;
            var today = DateTime.UtcNow.Date;

            var dailyMoodAverages = await unitOfWork.DbContext.Set<TestPeriodResult>()
                .Where(r => r.TestType == TestType.MOOD && !r.IsDeleted && r.CreatedAt.Date >= oneMonthAgo && r.CreatedAt.Date <= today)
                .GroupBy(r => r.CreatedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    AverageMood = g.Average(r => r.TotalScore)
                })
                .OrderBy(g => g.Date)
                .ToListAsync();

            var moodChartData = dailyMoodAverages.Select(d => new DataItem(
                Title: d.Date.ToString("yyyy/MM/dd"),
                Value: d.AverageMood.ToString("0.0"),
                DisplayValue: $"{GetMoodDescription(d.AverageMood)}"
                //DisplayValue: $"{d.AverageMood.ToString("0.0")} ({GetMoodDescription(d.AverageMood)})"
            )).ToList();

            var moodChart = new InfoChart("میانگین مود روزانه در یک ماه گذشته", "", false, false)
                .Add(new InfoSerie("میانگین مود", "").Add(moodChartData));

            result.Add(moodChart);
        }
        else if (userRole == "Mentor")
        {
            // گزارش برای منتور
            var mentorGroups = await unitOfWork.DbContext.Set<PatientGroup>()
                .Where(pg => pg.MentorId == userId && !pg.IsDeleted)
                .ToListAsync();

            var groupIds = mentorGroups.Select(g => g.Id).ToList();

            // تعداد بیماران در گروه‌های منتور
            var totalPatients = await unitOfWork.DbContext.Set<UserGroupMembership>()
                .CountAsync(ug => groupIds.Contains(ug.PatientGroupId) && !ug.IsDeleted);

            result.Add(new InfoSingleton(totalPatients.ToString(), "تعداد بیماران شما", ""));

            // تعداد گروه‌های درمانی منتور
            result.Add(new InfoSingleton(mentorGroups.Count.ToString(), "تعداد گروه‌های درمانی شما", ""));

            // تعداد جلسات منتور
            var totalSessions = await unitOfWork.DbContext.Set<CounselingSession>()
                .CountAsync(s => groupIds.Contains(s.PatientGroupId) && !s.IsDeleted);

            result.Add(new InfoSingleton(totalSessions.ToString(), "تعداد کل جلسات شما", ""));

            // اضافه کردن نمودار میانگین مود روزانه برای بیماران گروه‌های منتور
            var oneMonthAgo = DateTime.UtcNow.AddMonths(-1).Date;
            var today = DateTime.UtcNow.Date;

            var mentorPatientIds = mentorGroups
                .SelectMany(pg => pg.Members.Select(ugm => ugm.UserId))
                .ToList();

            var mentorDailyMoodAverages = await unitOfWork.DbContext.Set<TestPeriodResult>()
                .Where(r => r.TestType == TestType.MOOD &&
                            !r.IsDeleted &&
                            r.CreatedAt.Date >= oneMonthAgo &&
                            r.CreatedAt.Date <= today &&
                            mentorPatientIds.Contains(r.UserId))
                .GroupBy(r => r.CreatedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    AverageMood = g.Average(r => r.TotalScore)
                })
                .OrderBy(g => g.Date)
                .ToListAsync();

            var mentorMoodChartData = mentorDailyMoodAverages.Select(d => new DataItem(
                Title: d.Date.ToString("yyyy/MM/dd"),
                Value: d.AverageMood.ToString("0.0"),
                DisplayValue: $"{GetMoodDescription(d.AverageMood)}"
                //DisplayValue: $"{d.AverageMood.ToString("0.0")} ({GetMoodDescription(d.AverageMood)})"
            )).ToList();

            var mentorMoodChart = new InfoChart("میانگین مود روزانه در یک ماه گذشته (گروه‌های شما)", "", false, false)
                .Add(new InfoSerie("میانگین مود", "").Add(mentorMoodChartData));

            result.Add(mentorMoodChart);
        }

        return result;
    }

    // متد کمکی برای توضیحات کیفی مود
    private string GetMoodDescription(double averageMood)
    {
        if (averageMood <= 0.5)
            return "خیلی بد";
        else if (averageMood <= 1.5)
            return "بد";
        else if (averageMood <= 2.5)
            return "معمولی";
        else if (averageMood <= 3.5)
            return "خوب";
        else
            return "خیلی خوب";
    }

    //private async Task<InfoModel> GetGeneralReport()
    //{
    //    var result = new InfoModel();

    //    // تعداد کل بیماران
    //    var totalPatients = await userRepository.GetPagedPatientsAsync(
    //        new PagingInfo { PageNumber = 1, PageSize = int.MaxValue }, null, "");
    //    result.Add(new InfoSingleton(totalPatients.Count.ToString(), "تعداد کل بیماران", ""));

    //    // تعداد کل منتورها
    //    var totalMentors = await userRepository.GetPagedMentorsAsync(
    //         new PagingInfo { PageNumber = 1, PageSize = int.MaxValue }, null);
    //    result.Add(new InfoSingleton(totalMentors.Count.ToString(), "تعداد کل منتورها", ""));

    //    // تعداد کل گروه‌های درمانی
    //    var totalGroups = await unitOfWork.DbContext.Set<PatientGroup>()
    //        .CountAsync(g => !g.IsDeleted);

    //    result.Add(new InfoSingleton(totalGroups.ToString(), "تعداد کل گروه‌های درمانی", ""));

    //    // تعداد کل جلسات  
    //    var totalSessions = await unitOfWork.DbContext.Set<CounselingSession>()
    //        .CountAsync(s => !s.IsDeleted && s.ScheduledDate <= DateTime.UtcNow);

    //    result.Add(new InfoSingleton(totalSessions.ToString(), "تعداد کل جلسات", ""));

    //    // تعداد کل جلسات برگزار شده
    //    var totalSessionsHeld = await unitOfWork.DbContext.Set<CounselingSession>()
    //        .CountAsync(s => !s.IsDeleted && s.ScheduledDate <= DateTime.UtcNow);

    //    result.Add(new InfoSingleton(totalSessionsHeld.ToString(), "تعداد جلسات برگزار شده", ""));

    //    var totalSessionsNotHeld = await unitOfWork.DbContext.Set<CounselingSession>()
    //        .CountAsync(s => !s.IsDeleted && s.ScheduledDate > DateTime.UtcNow);

    //    result.Add(new InfoSingleton(totalSessionsNotHeld.ToString(), "تعداد جلسات برگزار نشده", ""));

    //    return result;
    //}

    // متد برای توصیف امتیازات GAD
    private string GetGadScoreDescription(int score)
    {
        if (score >= 0 && score <= 4) return "حداقل اضطراب";
        if (score >= 5 && score <= 9) return "اضطراب خفیف";
        if (score >= 10 && score <= 14) return "اضطراب متوسط";
        if (score >= 15) return "اضطراب شدید";
        return "-";
    }

    // متد برای توصیف امتیازات MDD
    private string GetMddScoreDescription(int score)
    {
        if (score >= 0 && score <= 4) return "حداقل خطر";
        if (score >= 5 && score <= 9) return "افسردگی خفیف";
        if (score >= 10 && score <= 14) return "افسردگی متوسط";
        if (score >= 15 && score <= 19) return "افسردگی نسبتاً شدید";
        if (score >= 20) return "افسردگی شدید";
        return "-";
    }


    private async Task<InfoModel> GetPatientDailyMoodReportByUserId(string userId)
    {
        var result = new InfoModel();

        // مقداردهی اولیه ExtraData
        result.ExtraData = new Dictionary<string, object>();

        // تاریخ امروز
        var today = DateTime.UtcNow.Date;

        // محاسبه تعداد حالات مود در یک ماه گذشته
        var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);

        // فیلتر کردن تست‌های مود روزانه در یک ماه گذشته
        var moodResults = await unitOfWork.DbContext.Set<TestPeriodResult>()
            .Where(r =>
                r.UserId == userId &&
                r.TestType == TestType.MOOD &&
                !r.IsDeleted &&
                r.CreatedAt >= oneMonthAgo)
            .OrderBy(r => r.CreatedAt) // مرتب‌سازی صعودی برای نمایش در نمودار
            .ToListAsync();

        // بررسی اینکه آیا مود امروز ثبت شده است یا خیر
        var todayMood = await unitOfWork.DbContext.Set<TestPeriodResult>()
            .FirstOrDefaultAsync(r =>
                r.UserId == userId &&
                r.TestType == TestType.MOOD &&
                !r.IsDeleted &&
                r.CreatedAt.Date == today);

        // محاسبه تعداد حالات مود
        var veryGoodCount = moodResults.Count(r => r.TotalScore == 4); // خیلی خوب
        var goodCount = moodResults.Count(r => r.TotalScore == 3); // خوب
        var neutralCount = moodResults.Count(r => r.TotalScore == 2); // متوسط
        var badCount = moodResults.Count(r => r.TotalScore == 1); // بد
        var veryBadCount = moodResults.Count(r => r.TotalScore == 0); // خیلی بد

        // افزودن تعدادها به گزارش
        result.Add(new InfoSingleton(veryGoodCount.ToString(), "تعداد خیلی خوب در یک ماه گذشته", ""));
        result.Add(new InfoSingleton(goodCount.ToString(), "تعداد خوب در یک ماه گذشته", ""));
        result.Add(new InfoSingleton(neutralCount.ToString(), "تعداد متوسط در یک ماه گذشته", ""));
        result.Add(new InfoSingleton(badCount.ToString(), "تعداد بد در یک ماه گذشته", ""));
        result.Add(new InfoSingleton(veryBadCount.ToString(), "تعداد خیلی بد در یک ماه گذشته", ""));

        // بررسی و افزودن وضعیت ثبت مود امروز
        if (todayMood is not null)
        {
            result.ExtraData["TodayMood"] = new
            {
                Score = todayMood.TotalScore,
                Description = GetMoodScoreDescription(todayMood.TotalScore),
                IsRecorded = true
            };
        }
        else
        {
            result.ExtraData["TodayMood"] = new
            {
                Score = 0,
                Description = "مود امروز ثبت نشده است.",
                IsRecorded = false
            };
        }

        // آماده‌سازی داده‌های نمودار
        var moodChartData = moodResults
            .Select(r => new DataItem(
                r.CreatedAt.ToString("yyyy/MM/dd"), // تاریخ
                r.TotalScore.ToString(), // امتیاز
                GetMoodScoreDescription(r.TotalScore), // توضیح کیفی مود
                null))
            .ToList();

        // افزودن نمودار مود روزانه به گزارش
        result.Add(
            new InfoChart("نمودار مود روزانه در یک ماه گذشته", "", false, false)
                .Add(new InfoSerie("مود روزانه", "").Add(moodChartData))
        );

        return result;
    }


    //private async Task<InfoModel> GetPatientDailyMoodReportByUserId(string userId)
    //{
    //    var result = new InfoModel();

    //    // محاسبه تعداد حالات مود در یک ماه گذشته
    //    var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);

    //    // فیلتر کردن تست‌های مود روزانه در یک ماه گذشته
    //    var moodResults = await unitOfWork.DbContext.Set<TestPeriodResult>()
    //        .Where(r =>
    //            r.UserId == userId &&
    //            r.TestType == TestType.MOOD &&
    //            !r.IsDeleted &&
    //            r.CreatedAt >= oneMonthAgo)
    //        .OrderBy(r => r.CreatedAt) // مرتب‌سازی صعودی برای نمایش در نمودار
    //        .ToListAsync();

    //    // محاسبه تعداد حالات مود
    //    var veryGoodCount = moodResults.Count(r => r.TotalScore == 4); // خیلی خوب
    //    var goodCount = moodResults.Count(r => r.TotalScore == 3); // خوب
    //    var neutralCount = moodResults.Count(r => r.TotalScore == 2); // متوسط
    //    var badCount = moodResults.Count(r => r.TotalScore == 1); // بد
    //    var veryBadCount = moodResults.Count(r => r.TotalScore == 0); // خیلی بد

    //    // افزودن تعدادها به گزارش
    //    result.Add(new InfoSingleton(veryGoodCount.ToString(), "تعداد خیلی خوب در یک ماه گذشته", ""));
    //    result.Add(new InfoSingleton(goodCount.ToString(), "تعداد خوب در یک ماه گذشته", ""));
    //    result.Add(new InfoSingleton(neutralCount.ToString(), "تعداد متوسط در یک ماه گذشته", ""));
    //    result.Add(new InfoSingleton(badCount.ToString(), "تعداد بد در یک ماه گذشته", ""));
    //    result.Add(new InfoSingleton(veryBadCount.ToString(), "تعداد خیلی بد در یک ماه گذشته", ""));

    //    // آماده‌سازی داده‌های نمودار
    //    var moodChartData = moodResults
    //        .Select(r => new DataItem(
    //            r.CreatedAt.ToString("yyyy/MM/dd"), // تاریخ
    //            r.TotalScore.ToString(), // امتیاز
    //            GetMoodScoreDescription(r.TotalScore), // توضیح کیفی مود
    //            null))
    //        .ToList();

    //    // افزودن نمودار مود روزانه به گزارش
    //    result.Add(
    //        new InfoChart("نمودار مود روزانه در یک ماه گذشته", "", false, false)
    //            .Add(new InfoSerie("مود روزانه", "").Add(moodChartData))
    //    );

    //    return result;
    //}

    // توضیحات کیفی مود روزانه
    private string GetMoodScoreDescription(int score)
    {
        return score switch
        {
            4 => "خیلی خوب",
            3 => "خوب",
            2 => "معمولی",
            1 => "بد",
            0 => "خیلی بد",
            _ => "نامشخص"
        };
    }


    private async Task<InfoModel> GetPatientMentalReport()
    {
        var result = new InfoModel();

        // تعداد تست‌های GAD
        var gadCount = await unitOfWork.DbContext.Set<TestPeriodResult>()
            .Where(t => t.TestType == TestType.GAD)
            .CountAsync();
        result.Add(new InfoSingleton(gadCount.ToString(), "تعداد تست GAD", ""));

        // تعداد تست‌های MDD
        var mddCount = await unitOfWork.DbContext.Set<TestPeriodResult>()
            .Where(t => t.TestType == TestType.MDD)
            .CountAsync();
        result.Add(new InfoSingleton(mddCount.ToString(), "تعداد تست MDD", ""));

        // گرفتن 20 نتیجه آخر
        var last20Results = await unitOfWork.DbContext.Set<TestPeriodResult>()
            .OrderByDescending(t => t.CreatedAt)
            .Take(20)
            .Select(t => new
            {
                t.CreatedAt,
                t.TotalScore,
                TestType = t.TestType.ToString()
            })
            .ToListAsync();

        // تبدیل تاریخ میلادی به شمسی و اضافه کردن به گزارش
        var pc = new PersianCalendar();
        var chartData = last20Results.Select(t =>
        {
            var date = t.CreatedAt;
            var dateStr = $"{pc.GetYear(date)}/{pc.GetMonth(date)}/{pc.GetDayOfMonth(date)}";
            return new DataItem(dateStr, t.TotalScore.ToString(), t.TestType, null);
        }).ToList();

        result.Add(new InfoChart("20 نتیجه آخر تست‌های روانشناسی", "", false, false)
            .Add(new InfoSerie("تست‌های روانشناسی", "").Add(chartData)));

        return result;
    }

}