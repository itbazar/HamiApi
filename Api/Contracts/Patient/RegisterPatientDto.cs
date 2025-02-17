using Application.Common.Interfaces.Security;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Patient
{
    public record RegisterPatientDto(
    //[Required] [RegularExpression(@"^09[0-9]{9}$")]
    string? PhoneNumber,
    string? Username,
    string? GroupName,
    string? Password,
    string? NationalId,
    string? FirstName,
    string? LastName,
    string? Title,
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
    int MDDScore,
    RoleType RoleType,
    bool IsSmoker);
}
