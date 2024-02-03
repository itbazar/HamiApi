using Application.Common.Interfaces.Security;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record ChangePhoneNumberRequestDto(
    [Required] [RegularExpression(@"^09[0-9]{9}$")]
    string NewPhoneNumber,
    CaptchaValidateModel Captcha);
