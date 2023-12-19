using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record ResetPasswordDto(
    [Required] [MaxLength(11)] [Phone]
    string Username,
    [Required] [MaxLength(1024)]
    string ResetPasswordToken,
    [Required] [MinLength(6)] [MaxLength(512)]
    string NewPassword);
