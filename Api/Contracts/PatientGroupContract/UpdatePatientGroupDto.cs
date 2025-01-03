using Domain.Models.Hami;

namespace Api.Contracts.PatientGroupContract;

public record UpdatePatientGroupDto(
    Organ? Organ,
    DiseaseType? DiseaseType,
    int? Stage,
    string? Description,
    string? MentorId);
