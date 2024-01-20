using Domain.Models.IdentityAggregate;
using Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    public Task<ApplicationUser> GetOrCreateCitizen(string phoneNumber, string firstName, string lastName);
    public Task<List<ApplicationUser>> GetUsersInRole(string roleName);
    public Task<Result<List<string>>> GetUserRoles(string userId);
    public Task<List<ApplicationRole>> GetRoles();
    public Task<bool> IsInRoleAsync(ApplicationUser user, string role);
    public Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    public Task<IdentityResult> DeleteAsync(ApplicationUser user);

    public Task<IdentityResult> AddToRolesAsync(ApplicationUser user, string[] roles);
    public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
    public Task<bool> UpdateRolesAsync(string userId, List<string> roles);
    public Task<ApplicationUser?> FindByNameAsync(string username);
    public Task<ApplicationUser?> FindByIdAsync(string id);
    public Task<ApplicationRole?> FindRoleByNameAsync(string roleName);
    public Task<IdentityResult> CreateRoleAsync(ApplicationRole applicationRole);
    public Task<bool> RoleExistsAsync(string roleName);
    public Task<bool> CreateNewPasswordAsync(string userId, string password);
    public Task<bool> Update(ApplicationUser user);
    public Task<PagedList<ApplicationUser>> GetPagedAsync(
        PagingInfo paging,
        Expression<Func<ApplicationUser, bool>>? filter = null);
}
