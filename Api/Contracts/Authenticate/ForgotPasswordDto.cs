using Application.Common.Interfaces.Security;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record ForgotPasswordDto(
    [Required] [MaxLength(11)] [Phone]
    string PhoneNumber,
    CaptchaValidateModel Captcha);
