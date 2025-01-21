using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Application.CounselingSessions.Queries.GetSessionAttendanceLogsQuery;

public record GetSessionAttendanceLogsQuery(Guid SessionId) : IRequest<Result<List<SessionAttendanceLog>>>;
