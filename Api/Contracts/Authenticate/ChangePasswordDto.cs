using Application.Common.Interfaces.Security;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record ChangePasswordDto(
    //string Username,
    [Required] [MinLength(6)] [MaxLength(512)]
    string OldPassword,
    [Required] [MinLength(6)] [MaxLength(512)]
    string NewPassword,
    CaptchaValidateModel Captcha);
