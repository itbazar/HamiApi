using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record RequestTokenDto(
    [Required] [MaxLength(11)] [Phone]
    string PhoneNumber,
    [Required] [MaxLength(8)]
    string VerificationCode);
