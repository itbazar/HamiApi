using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Domain.Models.ComplaintAggregate;
using Domain.Models.Hami;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Errors;
using System.Linq.Expressions;
using System.Linq;

namespace Infrastructure.Persistence.Repositories;

public class TestPeriodRepository(
    ApplicationDbContext context) : ITestPeriodRepository
{
    public async Task<Result<bool>> Insert(TestPeriod info)
    {
        context.Add(info);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Result<bool>> Update(TestPeriod info)
    {
        context.Update(info);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Result<TestPeriod>> GetAsyncByCode(int code)
    {
        var test = await context.TestPeriod
            .Where(c => c.Code == code)
            .SingleOrDefaultAsync();
        if (test is null)
            return TestPeriodErrors.NotFound;
        return test;
    }

}

