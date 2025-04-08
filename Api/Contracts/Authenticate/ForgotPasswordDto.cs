using Application.Common.Interfaces.Security;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record ForgotPasswordDto(
    [Required] [MaxLength(11)] [Phone]
    string PhoneNumber,
    CaptchaValidateModel Captcha);


public record ResendOtpDto(
    [Required]
    string OtpToken);

public record ResetForgetPasswordDto(
    string OtpToken,
    string VerificationCode,
    [Required] [MinLength(6)] [MaxLength(512)]
    string NewPassword);

