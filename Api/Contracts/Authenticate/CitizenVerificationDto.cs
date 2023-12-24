using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record CitizenVerificationDto(
    [RegularExpression(@"^09[0-9]{9}$")]
    string PhoneNumber,
    [Required] [MaxLength(8)]
    string VerificationCode);