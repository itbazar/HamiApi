using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record UpdateCitizenProfileDto(
    [MaxLength(32)]
    string? FirstName,
    [MaxLength(32)]
    string? LastName,
    [MaxLength(11)]
    string? PhoneNumber2,
    [MaxLength(16)]
    string? NationalId);
