using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record StaffVerificationDto(
    string OtpToken,
    [Required] [MaxLength(8)]
    string VerificationCode);
