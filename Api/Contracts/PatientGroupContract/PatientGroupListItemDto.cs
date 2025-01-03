using Domain.Models.Common;
using Domain.Models.Hami;

namespace Api.Contracts.PatientGroupContract;

public record PatientGroupListItemDto(
    Guid Id,
    Organ Organ,
    DiseaseType DiseaseType,
    int Stage,
    string Description,
    string MentorId);