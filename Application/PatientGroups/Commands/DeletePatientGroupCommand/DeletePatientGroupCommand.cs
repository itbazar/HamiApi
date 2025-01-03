using Domain.Models.Hami;
using MediatR;

namespace Application.PatientGroups.Commands.DeletePatientGroupCommand;

public record DeletePatientGroupCommand(
    Guid Id,
    bool? IsDeleted = null) : IRequest<Result<PatientGroup>>;
