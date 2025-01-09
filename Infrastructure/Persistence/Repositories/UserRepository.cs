using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Errors;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository(
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager) : IUserRepository
{
    public async Task<ApplicationUser> GetOrCreateCitizen(string phoneNumber, string firstName, string lastName)
    {
        var user = await userManager.FindByNameAsync(phoneNumber);
        if (user == null)
        {
            user = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = phoneNumber,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = false,
                FirstName = firstName,
                LastName = lastName
            };
            //TODO: Generate password randomly
            var password = "aA@12345";
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception("User creation failed.", null);

            var result2 = await userManager.AddToRoleAsync(user, "Citizen");

            if (!result2.Succeeded)
            {
                await userManager.DeleteAsync(user);
                throw new Exception("Role assignment failed.", null);
            }

            //TODO: Send the user its credentials
        }

        return user;
    }

    public async Task<List<ApplicationRole>> GetRoleActors(List<string> ids)
    {
        var result = await dbContext.Roles.Where(p => ids.Contains(p.Id)).AsNoTracking().ToListAsync();
        return result;
    }

    public async Task<List<ApplicationUser>> GetUserActors(List<string> ids)
    {
        var result = await dbContext.Users.Where(p => ids.Contains(p.Id)).AsNoTracking().ToListAsync();
        return result;
    }

    public async Task<List<ApplicationRole>> GetRoles()
    {
        var result = await dbContext.Roles.ToListAsync();
        return result;
    }

    public async Task<List<ApplicationUser>> GetUsersInRole(string roleName)
    {
        var result = await dbContext.Roles.Where(r => r.Name == roleName)
            .Join(dbContext.UserRoles, r => r.Id, ur => ur.RoleId, (r, ur) => new { ur.UserId })
            .Join(dbContext.Users, uid => uid.UserId, u => u.Id, (uid, u) => u )
            .ToListAsync();
        if(result is null)
            return new List<ApplicationUser>();
        return result;
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        var result = await userManager.CreateAsync(user, password);
        return result;
    }

    public async Task<IdentityResult> AddToRolesAsync(ApplicationUser user, string[] roles)
    {
        var result = await userManager.AddToRolesAsync(user, roles);
        return result;
    }

    public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
    {
        var result = await userManager.AddToRoleAsync(user, role);
        return result;
    }

    public async Task<ApplicationUser?> FindByNameAsync(string username)
    {
        var result = await userManager.FindByNameAsync(username);
        return result;
    }

    public async Task<ApplicationRole?> FindRoleByNameAsync(string roleName)
    {
        var result = await roleManager.FindByNameAsync(roleName);
        return result;
    }

    public async Task<IdentityResult> CreateRoleAsync(ApplicationRole applicationRole)
    {
        var result = await roleManager.CreateAsync(applicationRole);
        return result;
    }

    public async Task<bool> RoleExistsAsync(string roleName)
    {
        var result = await roleManager.RoleExistsAsync(roleName);
        return result;
    }

    public async Task<bool> CreateNewPasswordAsync(string userId, string password)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return false;
        var result = await userManager.RemovePasswordAsync(user);
        if (result is null || !result.Succeeded)
            return false;
        result = await userManager.AddPasswordAsync(user, password);
        if (result is null || !result.Succeeded)
            return false;
        return true;
    }

    public async Task<bool> UpdateRolesAsync(string userId, List<string> roles)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return false;
        var currentRoles = await userManager.GetRolesAsync(user);
        var inRoles = roles.Where(r => !currentRoles.Contains(r)).ToList();
        var outRoles = currentRoles.Where(r => !roles.Contains(r)).ToList();
        var result = await userManager.RemoveFromRolesAsync(user, outRoles);
        if (result is null || !result.Succeeded)
            return false;
        var result2 = await userManager.AddToRolesAsync(user, inRoles);
        if(result2 is null || !result2.Succeeded)
        {
            //try to rollback operation
            //TODO: This won't work in all cases. What if these commands fail?
            await userManager.AddToRolesAsync(user, outRoles);
            return false;
        }
            
        return true;
    }

    public async Task<Result<List<string>>> GetUserRoles(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return UserErrors.UserNotExsists;
        var roles = await userManager.GetRolesAsync(user);
        if (roles is null)
            return UserErrors.UnExpected;
        return roles.ToList();
    }

    public async Task<bool> IsInRoleAsync(ApplicationUser user, string role)
    {
        var result = await userManager.IsInRoleAsync(user, role);
        return result;
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationUser user)
    {
        var result = await userManager.DeleteAsync(user);
        return result;
    }

    public async Task<ApplicationUser?> FindByIdAsync(string id)
    {
        var result = await userManager.FindByIdAsync(id);
        return result;
    }

    public async Task<bool> Update(ApplicationUser user)
    {
        dbContext.Update(user);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<PagedList<ApplicationUser>> GetPagedAsync(
        PagingInfo paging,
        Expression<Func<ApplicationUser, bool>>? filter = null)
    {
        var query = dbContext.Users.Where(u => true);
        if (filter is not null)
            query = query.Where(filter);
        
        return await PagedList<ApplicationUser>.ToPagedList(query, paging.PageNumber, paging.PageSize);
    }

    public async Task<PagedList<ApplicationUser>> GetPagedPatientsAsync(
    PagingInfo paging,
    RegistrationStatus? Status,
    Expression<Func<ApplicationUser, bool>>? filter = null)
    {
        // ابتدا کاربران با نقش Patient را فیلتر می‌کنیم
        var query = from user in dbContext.Users
                    join userRole in dbContext.UserRoles on user.Id equals userRole.UserId
                    join role in dbContext.Roles on userRole.RoleId equals role.Id
                    where role.Name == "Patient" // نقش Patient
                    select user;

        // اگر فیلتری وجود داشت، آن را اعمال می‌کنیم
        if (filter is not null)
            query = query.Where(filter);

        if (Status is not null)
            query = query.Where(user => user.RegistrationStatus == Status.Value);

        // بازگشت لیست صفحه‌بندی‌شده
        return await PagedList<ApplicationUser>.ToPagedList(query, paging.PageNumber, paging.PageSize);
    }

}
