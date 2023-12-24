using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record UpdateProfileDto(
    [MaxLength(32)]
    string? FirstName,
    [MaxLength(32)]
    string? LastName,
    [MaxLength(16)]
    string? NationalId,
    [MaxLength(11)]
    string? PhoneNumber2,
    [MaxLength(32)]
    string? Title);
