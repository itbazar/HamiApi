using Domain.Models.Hami;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.PatientGroups.Commands.AddPatientGroupCommand;

public record AddPatientGroupCommand(
    Organ Organ,
    DiseaseType DiseaseType,
    int? Stage,
    string Description,
    string? MentorId) : IRequest<Result<PatientGroup>>;
