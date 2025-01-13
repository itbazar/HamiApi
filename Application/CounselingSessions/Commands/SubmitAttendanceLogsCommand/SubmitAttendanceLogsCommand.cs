using Domain.Models.Hami;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.CounselingSessions.Commands.SubmitAttendanceLogsCommand;

public record SubmitAttendanceLogsCommand(
   Guid SessionId,
   List<SessionAttendanceLog> AttendanceLogs) : IRequest<Result<CounselingSession>>;
