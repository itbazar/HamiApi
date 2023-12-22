using Application.Common.Interfaces.Persistence;
using Domain.Models.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UserRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager) : base(dbContext)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ApplicationUser> GetOrCreateCitizen(string phoneNumber, string firstName, string lastName)
    {
        var user = await _userManager.FindByNameAsync(phoneNumber);
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
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception("User creation failed.", null);

            var result2 = await _userManager.AddToRoleAsync(user, "Citizen");

            if (!result2.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                throw new Exception("Role assignment failed.", null);
            }

            //TODO: Send the user its credentials
        }

        return user;
    }

    public async Task<List<ApplicationRole>> GetRoleActors(List<string> ids)
    {
        var result = await _dbContext.Roles.Where(p => ids.Contains(p.Id)).AsNoTracking().ToListAsync();
        return result;
    }

    public async Task<List<ApplicationUser>> GetUserActors(List<string> ids)
    {
        var result = await _dbContext.Users.Where(p => ids.Contains(p.Id)).AsNoTracking().ToListAsync();
        return result;
    }

    public async Task<List<ApplicationRole>> GetRoles()
    {
        var result = await _dbContext.Roles.ToListAsync();
        return result;
    }

    public async Task<List<ApplicationUser>> GetUsersInRole(string roleName)
    {
        var result = await _dbContext.Roles.Where(r => r.Name == roleName)
            .Join(_dbContext.UserRoles, r => r.Id, ur => ur.RoleId, (r, ur) => new { ur.UserId })
            .Join(_dbContext.Users, uid => uid.UserId, u => u.Id, (uid, u) => u )
            .ToListAsync();
        if(result is null)
            return new List<ApplicationUser>();
        return result;
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        return result;
    }

    public async Task<IdentityResult> AddToRolesAsync(ApplicationUser user, string[] roles)
    {
        var result = await _userManager.AddToRolesAsync(user, roles);
        return result;
    }

    public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
    {
        var result = await _userManager.AddToRoleAsync(user, role);
        return result;
    }

    public async Task<ApplicationUser?> FindByNameAsync(string username)
    {
        var result = await _userManager.FindByNameAsync(username);
        return result;
    }

    public async Task<ApplicationRole?> FindRoleByNameAsync(string roleName)
    {
        var result = await _roleManager.FindByNameAsync(roleName);
        return result;
    }

    public async Task<IdentityResult> CreateRoleAsync(ApplicationRole applicationRole)
    {
        var result = await _roleManager.CreateAsync(applicationRole);
        return result;
    }

    public async Task<bool> RoleExistsAsync(string roleName)
    {
        var result = await _roleManager.RoleExistsAsync(roleName);
        return result;
    }

    public async Task<bool> CreateNewPasswordAsync(string userId, string password)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            throw new Exception("User not found.");
        var result = await _userManager.RemovePasswordAsync(user);
        if (result is null || !result.Succeeded)
            return false;
        result = await _userManager.AddPasswordAsync(user, password);
        if (result is null || !result.Succeeded)
            return false;
        return true;
    }

    public async Task<bool> UpdateRolesAsync(string userId, List<string> roles)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            throw new Exception("User not found");
        var currentRoles = await _userManager.GetRolesAsync(user);
        var inRoles = roles.Where(r => !currentRoles.Contains(r)).ToList();
        var outRoles = currentRoles.Where(r => !roles.Contains(r)).ToList();
        var result = await _userManager.RemoveFromRolesAsync(user, outRoles);
        if (result is null || !result.Succeeded)
            return false;
        var result2 = await _userManager.AddToRolesAsync(user, inRoles);
        if(result2 is null || !result2.Succeeded)
        {
            //try to rollback operation
            //TODO: This won't work in all cases. What if these commands fail?
            await _userManager.AddToRolesAsync(user, outRoles);
            return false;
        }
            
        return true;
    }

    public async Task<List<string>> GetUserRoles(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            throw new Exception("User not found.");
        var roles = await _userManager.GetRolesAsync(user);
        if (roles is null)
            throw new Exception("Unknown problem!");
        return roles.ToList();
    }

    public async Task<bool> IsInRoleAsync(ApplicationUser user, string role)
    {
        var result = await _userManager.IsInRoleAsync(user, role);
        return result;
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);
        return result;
    }
}
