using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record ChangePasswordAppDto(
    [Required] [MinLength(6)] [MaxLength(512)]
    string OldPassword,
    [Required] [MinLength(6)] [MaxLength(512)]
    string NewPassword);
