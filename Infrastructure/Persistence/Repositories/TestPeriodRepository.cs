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

public class TestPeriodRepository : GenericRepository<TestPeriod>, ITestPeriodRepository
{
    private readonly ApplicationDbContext _context;

    public TestPeriodRepository(ApplicationDbContext context) : base(context) // ارسال context به کلاس پدر
    {
        _context = context; // نگهداری یک کپی از context در این کلاس 
    }
    public async Task<Result<TestPeriod>> GetAsyncByCode(int code)
    {
        var test = await _context.TestPeriod
            .Where(c => c.Code == code)
            .SingleOrDefaultAsync();
        if (test is null)
            return TestPeriodErrors.NotFound;
        return test;
    }

}

