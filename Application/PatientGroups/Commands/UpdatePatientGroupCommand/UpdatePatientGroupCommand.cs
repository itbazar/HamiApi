using Domain.Models.Hami;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.PatientGroups.Commands.UpdatePatientGroupCommand;

public record UpdatePatientGroupCommand(
    Guid Id,
    Organ? Organ,
    DiseaseType? DiseaseType,
    int? Stage,
    string? Description,
    string? MentorId) : IRequest<Result<PatientGroup>>;
