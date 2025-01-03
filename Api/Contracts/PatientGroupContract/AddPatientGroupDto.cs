using Domain.Models.Hami;

namespace Api.Contracts.PatientGroupContract;

public record AddPatientGroupDto(
    Organ Organ,
    DiseaseType DiseaseType,
    int? Stage,
    string Description="",
    string? MentorId="");