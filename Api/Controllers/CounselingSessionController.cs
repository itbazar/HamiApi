using Api.Abstractions;
using Api.Contracts.CounselingSessionContract;
using Api.Contracts.PatientGroupContract;
using Api.Contracts.TestPeriodResultContract;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.CounselingSessionApp.Queries.GetCounselingSessionByIdQuery;
using Application.CounselingSessions.Commands.AddCounselingSessionCommand;
using Application.CounselingSessions.Commands.DeleteCounselingSessionCommand;
using Application.CounselingSessions.Commands.SubmitAttendanceLogsCommand;
using Application.CounselingSessions.Commands.UpdateCounselingSessionCommand;
using Application.CounselingSessions.Queries.GetCounselingSessionQuery;
using Application.CounselingSessions.Queries.GetMentorCounselingSessionsQuery;
using Application.Questions.Queries.GetMentorPatientGroupsQuery;
using Application.Users.Common;
using Domain.Models.Hami;
using Mapster;
using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Api.Controllers;

[Authorize(Roles = "Admin,Mentor")]
public class CounselingSessionController : ApiController
{
    public CounselingSessionController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    public async Task<ActionResult<List<CounselingSessionListItemDto>>> GetCounselingSessionList([FromQuery] PagingInfo pagingInfo)
    {
        var query = new GetCounselingSessionQuery(pagingInfo);
        var result = await Sender.Send(query);
        if (result.IsFailed)
            return Problem(result.ToResult());

        Response.AddPaginationHeaders(result.Value.Meta);

        // مپ کردن مقادیر به TestPeriodResultListItemDto
        var dtoList = result.Value.Select(x => new CounselingSessionListItemDto(
            x.Id,
            x.PatientGroupId,
            x.PatientGroup.Description ?? "نامشخص",
            x.MentorId,
            x.Mentor.FirstName + " " + x.Mentor.LastName ?? "نامشخص", // نام کاربر
            x.ScheduledDate,
            x.Topic,
            x.MeetingLink,
            x.IsConfirmed,
            x.MentorNote,
            false
        )).ToList();

        return Ok(dtoList);
    }


    [Authorize(Roles = "Mentor")]
    [HttpGet("MentorSessions")]
    public async Task<ActionResult<List<CounselingSessionListItemDto>>> GetMentorSessions([FromQuery] Guid? groupId)
    {
        var userId = User.GetUserId();
        var query = new GetMentorCounselingSessionsQuery(userId, groupId);
        var result = await Sender.Send(query);
        if (result.IsFailed)
            return Problem(result.ToResult());

        // مپ کردن مقادیر به TestPeriodResultListItemDto
        var dtoList = result.Value.Select(x => new CounselingSessionListItemDto(
            x.Id,
            x.PatientGroupId,
            x.PatientGroup.Description ?? "نامشخص",
            x.MentorId,
            x.Mentor.FirstName + " " + x.Mentor.LastName ?? "نامشخص", // نام کاربر
            x.ScheduledDate,
            x.Topic,
            x.MeetingLink,
            x.IsConfirmed,
            x.MentorNote,
            false
        )).ToList();

        return Ok(dtoList);
    }


    [HttpGet("SessionUsers/{sessionId}")]
    public async Task<ActionResult<List<AttendanceLogDto>>> GetSessionUsers(Guid sessionId)
    {
        var query = new GetSessionUsersQuery(sessionId);
        var result = await Sender.Send(query);
        if (result.IsFailed)
            return Problem(result.ToResult());

        var dtoList = result.Value.Select(x => new AttendanceLogDto(
            x.Id,
            x.UserName,
            x.FirstName, 
            x.LastName,
            true,
            " "
        )).ToList();

        return Ok(dtoList);
    }


    [HttpPost("SubmitAttendanceLogs")]
    public async Task<IActionResult> SubmitAttendanceLogs([FromBody] SubmitAttendanceLogsDto dto)
    {
        // تبدیل AttendanceLogDto به SessionAttendanceLog
        var attendanceLogs = dto.AttendanceLogs.Select(log =>
            SessionAttendanceLog.Create(dto.SessionId, log.UserId, log.Attended, log.MentorNote)).ToList();

        var command = new SubmitAttendanceLogsCommand(dto.SessionId, attendanceLogs);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CounselingSession>> GetCounselingSession(Guid id)
    {
        var query = new GetCounselingSessionByIdQuery(id);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost]
    public async Task<ActionResult> AddCounselingSession([FromForm] AddCounselingSessionDto counselingSessionDto)
    {
        var command = new AddCounselingSessionCommand(
            counselingSessionDto.PatientGroupId,
            counselingSessionDto.MentorId,
            counselingSessionDto.ScheduledDate,
            counselingSessionDto.Topic,
            counselingSessionDto.MeetingLink,
            counselingSessionDto.MentorNote);

        var result = await Sender.Send(command);
        return result.Match(
            s => CreatedAtAction(nameof(GetCounselingSession), new { id = s.Id }, s),
            f => Problem(f));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateCounselingSession(Guid id, [FromForm] UpdateCounselingSessionDto counselingSessionDto)
    {
        var command = new UpdateCounselingSessionCommand(
            id,
            counselingSessionDto.MentorId,
            counselingSessionDto.ScheduledDate,
            counselingSessionDto.Topic,
            counselingSessionDto.MeetingLink,
            counselingSessionDto.MentorNote);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteCounselingSession(Guid id)
    {
        var command = new DeleteCounselingSessionCommand(id);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }
}
