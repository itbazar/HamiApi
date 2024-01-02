using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        return await GetCategoryHistogram();
    }    

    private async Task<InfoModel> GetCategoryHistogram()
    {
        var categories = await _unitOfWork.DbContext.Set<ComplaintCategory>().ToListAsync();
        var query = _unitOfWork.DbContext.Set<Complaint>().Where(d => true);
        var hist = await query.GroupBy(c => c.CategoryId)
            .Select(grp => new { Id = grp.Key, Count = grp.Count() })
            .ToListAsync();

        var result = new InfoModel();
        var serie = new InfoSerie("", "");
        result.Add(new InfoChart("Title", "", false, false).Add(serie));


        foreach (var category in categories)
        {
            var item = hist.Where(h => h.Id == category.Id).FirstOrDefault();
            if (item is null)
            {
                serie.Add(category.Title, "0", "0");
            }
            else
            {
                serie.Add(category.Title, item.Count.ToString(), item.Count.ToString());
            }
        }
        return result;
    }

    private async Task<InfoModel> GetHistogram<T>(DbSet<T> valuesDbSet, Func<T, string> title, IQueryable<T> query, Expression<Func<T, Guid>> groupBy) where T : Entity
    {
        var values = await valuesDbSet.ToListAsync();
        var hist = await query.GroupBy(groupBy)
            .Select(grp => new { Id = grp.Key, Count = grp.Count() })
            .ToListAsync();

        var result = new InfoModel();
        var serie = new InfoSerie("", "");
        result.Add(new InfoChart("Title", "", false, false).Add(serie));


        foreach (var value in values)
        {
            var item = hist.Where(h => h.Id == value.Id).FirstOrDefault();
            if (item is null)
            {
                serie.Add(title.Invoke(value), "0", "0");
            }
            else
            {
                serie.Add(title.Invoke(value), item.Count.ToString(), item.Count.ToString());
            }
        }
        return result;
    }
}