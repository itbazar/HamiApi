using Application.Common.Interfaces.Security;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record LogisterCitizenDto(
    [Required] [RegularExpression(@"^09[0-9]{9}$")]
    string PhoneNumber,
    CaptchaValidateModel Captcha);