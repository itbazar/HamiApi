using Domain.Models.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Api.Services;

public class CustomPasswordValidator<TUser> : IPasswordValidator<TUser>
where TUser : ApplicationUser
{
    public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string? password)
    {
        var isValid = true;

        if (password == null)
            isValid = false;
        else if (password.Length < 6 || !Regex.IsMatch(password, @"[a-zA-Z]") || !Regex.IsMatch(password, @"[0-9]"))
            isValid = false;

        if (!isValid)
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "PasswordValidator",
                Description = "رمز عبور می بایست شامل حداقل یک حرف انگلیسی و یک رقم باشد."
            }));

        return Task.FromResult(IdentityResult.Success);
    }
}
