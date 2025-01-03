using Application.Users.Common;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;

namespace Application.Users.Commands.RegisterPatient;

public record RegisterPatientCommand(
    string Username,
    string Password,
    string PhoneNumber,
    string NationalId,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    Gender Gender,
    EducationLevel? Education,
    string? City,
    Organ Organ,
    DiseaseType DiseaseType,
    PatientStatus PatientStatus,
    int? Stage,
    string? PathologyDiagnosis,
    float? InitialWeight,
    int? SleepDuration,
    AppetiteLevel AppetiteLevel,
    int GADScore,
    int MDDScore) : IRequest<Result<AddPatientResult>>;
