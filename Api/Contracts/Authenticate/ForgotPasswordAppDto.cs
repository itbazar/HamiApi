using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record ForgotPasswordAppDto(
    [Required] [MaxLength(11)] [Phone]
    string PhoneNumber);
