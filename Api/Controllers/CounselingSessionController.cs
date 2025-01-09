﻿using Api.Abstractions;
using Api.Contracts.CounselingSessionContract;
using Api.Contracts.TestPeriodResultContract;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.CounselingSessionApp.Queries.GetCounselingSessionByIdQuery;
using Application.CounselingSessions.Commands.AddCounselingSessionCommand;
using Application.CounselingSessions.Commands.DeleteCounselingSessionCommand;
using Application.CounselingSessions.Commands.UpdateCounselingSessionCommand;
using Application.CounselingSessions.Queries.GetCounselingSessionQuery;
using Domain.Models.Hami;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Api.Controllers;

[Authorize(Roles = "Admin,Inspector")]
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
            x.MentorNote
        )).ToList();

        return Ok(dtoList);
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
