using Application.Users.Common;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;

namespace Application.Users.Commands.CreateMentor;

public record CreateMentorCommand(
    string Username,
    string Password,
    string PhoneNumber,
    string FirstName,
    string LastName,
    string Title,
    string Email,
    //DateTime DateOfBirth,
    Gender Gender,
    EducationLevel? Education,
    string? City) : IRequest<Result<AddPatientResult>>;
