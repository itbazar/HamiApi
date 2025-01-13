using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetPatientsSessionReport;

public record GetPatientsSessionReportQuery(string UserId) 
    : IRequest<Result<List<CounselingSession>>>;