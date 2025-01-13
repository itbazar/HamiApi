using Application.Common.Interfaces.Persistence;
using Application.Users.Common;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetPatientTestPeriodsReport;

public record GetPatientTestPeriodsReportQuery(string UserId) 
    : IRequest<Result<List<TestPeriodResponse>>>;