using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Domain.Models.ComplaintAggregate;
using Domain.Models.Hami;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Errors;
using System.Linq.Expressions;
using System.Linq;
using Domain.Models.IdentityAggregate;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Repositories;

public class UserGroupMembershipRepository : GenericRepository<UserGroupMembership> , IUserGroupMembershipRepository
{
    private readonly ApplicationDbContext context;

    public UserGroupMembershipRepository(ApplicationDbContext _context) : base(_context) // ارسال context به کلاس پدر
    {
        context = _context; // نگهداری یک کپی از context در این کلاس 
    }

    public async Task<Result<bool>> Insert(UserGroupMembership info)
    {
        context.Add(info);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Result<bool>> Update(UserGroupMembership info)
    {
        context.Update(info);

        await context.SaveChangesAsync();
        return true;
    }
    public async Task<Result<UserGroupMembership>> FindByUserIdAsync(string userId)
    {
        var result = await context.UserGroupMembership.Where(c => c.UserId == userId)
            .FirstOrDefaultAsync();
        if (result is null)
            return UserErrors.UserGroupNotAssigned;
        return result;
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

