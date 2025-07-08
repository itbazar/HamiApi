using Application.Common.Interfaces.Security;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Mentors
{
    public record UpdateMentorDto(
    string MentorId,
    string? PhoneNumber,
    string? Password,
    string? FirstName,
    string? LastName,
    string? Title,
    string? Email,
    //DateTime DateOfBirth,
    Gender Gender,
    EducationLevel? Education,
    string? City,
    RoleType? RoleType);
}
