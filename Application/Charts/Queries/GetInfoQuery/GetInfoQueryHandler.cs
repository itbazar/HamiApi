using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.ExtensionMethods;
using Domain.Models.ComplaintAggregate;
using Domain.Models.IdentityAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;

namespace Application.Charts.Queries.GetInfoQuery;

internal class GetInfoQueryHandler : IRequestHandler<GetInfoQuery, InfoModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetInfoQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<InfoModel> Handle(GetInfoQuery request, CancellationToken cancellationToken)
    {
        InfoModel result = new InfoModel();
        switch (request.Code)
        {
            case ChartCodes.Dashboard:
                result = await GetSummary();
                break;
            case ChartCodes.ComplaintCategoryHistogram:
                var bins = await _unitOfWork.DbContext.Set<ComplaintCategory>().Select(cc => new Bin<Guid>(cc.Id, cc.Title)).ToListAsync();
                result = await GetHistogram(
                    "فراوانی دسته بندی ها",
                    bins,
                    _unitOfWork.DbContext.Set<Complaint>(),
                    c => c.CategoryId); 
                break;
            case ChartCodes.ComplaintOrganizationHistogram:
                var bins3 = await _unitOfWork.DbContext.Set<ComplaintOrganization>().Select(co => new Bin<Guid?>(co.Id, co.Title)).ToListAsync();
                result = await GetHistogram(
                    "فراوانی سازمان ها",
                    bins3,
                    _unitOfWork.DbContext.Set<Complaint>(),
                    c => c.ComplaintOrganizationId);
                break;
            case ChartCodes.ComplaintStatusHistogram:
                var bins2 = GetBins<ComplaintState>();
                result = await GetHistogram(
                    "فراوانی وضعیت ها",
                    bins2,
                    _unitOfWork.DbContext.Set<Complaint>(),
                    c => c.Status);
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

        var anonymousComplaintsCount = await _unitOfWork.DbContext.Set<Complaint>()
            .Where(c => c.UserId == null)
            .CountAsync();
        result.Add(new InfoSingleton(anonymousComplaintsCount.ToString(), "درخواست ناشناس", ""));

        var knownComplaintsCount = await _unitOfWork.DbContext.Set<Complaint>()
            .Where(c => c.UserId != null)
            .CountAsync();
        result.Add(new InfoSingleton(knownComplaintsCount.ToString(), "درخواست شناس", ""));

        var totalComplaintsCount = anonymousComplaintsCount + knownComplaintsCount;
        result.Add(new InfoSingleton(totalComplaintsCount.ToString(), "درخواست", ""));

        var totalUsers = await _unitOfWork.DbContext.Set<ApplicationUser>().CountAsync();
        result.Add(new InfoSingleton(totalUsers.ToString(), "کاربر", ""));

        var now = DateTime.UtcNow;
        var query = _unitOfWork.DbContext.Set<Complaint>().Where(c => true);
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
        result.Add(new InfoChart("تعداد درخواست ثبت شده در سی روز گذشته", "", false, false).Add(new InfoSerie("درخواست", "").Add(prpd)));


        return result;
    }
}