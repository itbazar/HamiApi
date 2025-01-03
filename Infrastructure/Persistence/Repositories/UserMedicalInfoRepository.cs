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

public class UserMedicalInfoRepository(
    ApplicationDbContext context) : IUserMedicalInfoRepository
{
    public async Task<Result<bool>> Insert(UserMedicalInfo info)
    {
        context.Add(info);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Result<bool>> Update(UserMedicalInfo info)
    {
        context.Update(info);

        await context.SaveChangesAsync();
        return true;
    }

    //public async Task<Result<Complaint>> GetAsync(string trackingNumber)
    //{
    //    var complaint = await context.Complaint
    //        .Where(c => c.TrackingNumber == trackingNumber)
    //        .SingleOrDefaultAsync();
    //    if (complaint is null)
    //        return ComplaintErrors.NotFound;
    //    return complaint;
    //}

}

