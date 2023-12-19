using Application.Common.Interfaces.Security;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record LoginStaffDto(
    [Required] [MaxLength(32)]
    string Username,
    [Required] [MinLength(6)] [MaxLength(512)]
    string Password,
    CaptchaValidateModel Captcha);
