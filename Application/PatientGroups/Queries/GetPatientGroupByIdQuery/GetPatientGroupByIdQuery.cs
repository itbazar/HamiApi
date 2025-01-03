using Domain.Models.Hami;
using MediatR;

namespace Application.PatientGroupApp.Queries.GetPatientGroupByIdQuery;

public record GetPatientGroupByIdQuery(Guid Id) : IRequest<Result<PatientGroup>>;
