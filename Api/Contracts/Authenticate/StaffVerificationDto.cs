using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record StaffVerificationDto(
    string Username,
    [Required] [MinLength(6)] [MaxLength(512)]
    string Password,
    [Required] [MaxLength(8)]
    string VerificationCode);
