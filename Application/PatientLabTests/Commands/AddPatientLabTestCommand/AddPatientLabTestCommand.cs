using Domain.Models.Hami;
using MediatR;
using FluentResults;

namespace Application.PatientLabTests.Commands.AddPatientLabTestCommand;

public record AddPatientLabTestCommand(
    string UserId,         // شناسه بیمار
    LabTestType TestType,  // نوع آزمایش
    decimal TestValue,     // مقدار آزمایش
    string Unit           // واحد آزمایش (مثلاً ng/ml, U/ml)
) : IRequest<Result<PatientLabTest>>;
