using Domain.Models.Hami;
using MediatR;

namespace Application.PatientLabTests.Queries.GetPatientLabTestByIdQuery;

public record GetPatientLabTestByIdQuery(Guid Id) : IRequest<Result<PatientLabTest>>;
