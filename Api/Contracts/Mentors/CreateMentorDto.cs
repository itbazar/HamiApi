using Application.Common.Interfaces.Security;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Mentors
{
    public record CreateMentorDto(
    //[Required] [RegularExpression(@"^09[0-9]{9}$")]
    string? PhoneNumber,
    string? Username,
    string? Password,
    string? FirstName,
    string? LastName,
    string? Title,
    string? Email,
    //DateTime DateOfBirth,
    Gender Gender,
    EducationLevel? Education,
    string? City);
}
