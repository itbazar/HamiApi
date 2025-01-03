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

public class TestPeriodResultRepository(
    ApplicationDbContext context) : ITestPeriodResultRepository
{
    public async Task<Result<bool>> Insert(TestPeriodResult info)
    {
        context.Add(info);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Result<bool>> Update(TestPeriodResult info)
    {
        context.Update(info);

        await context.SaveChangesAsync();
        return true;
    }
}

