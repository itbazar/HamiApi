using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record CitizenVerificationDto(
    string OtpToken,
    [Required] [MaxLength(8)]
    string VerificationCode);