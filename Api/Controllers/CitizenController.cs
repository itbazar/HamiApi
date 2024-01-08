﻿using Api.Abstractions;
using Api.Contracts.Complaint;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.Complaints.Commands.AddComplaintCommand;
using Application.Complaints.Commands.ReplyComplaintCitizenCommand;
using Application.Complaints.Common;
using Application.Complaints.Queries.GetComplaintCitizenQuery;
using Application.Complaints.Queries.GetComplaintListQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class CitizenController : ApiController
{
    public CitizenController(ISender sender) : base(sender)
    {
    }

    [HttpPost]
    public async Task<IActionResult> AddComplaint([FromForm] ComplaintCreateDto createDto)
    {
        var command = new AddComplaintCommand(
            null,
            createDto.Title,
            createDto.Text,
            createDto.CategoryId,
            createDto.Medias.GetMedia(),
            createDto.Captcha,
            createDto.Complaining,
            createDto.OrganizationId);
        var result = await Sender.Send(command);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("Authorized")]
    public async Task<IActionResult> AddComplaintAuthorized([FromForm] ComplaintCreateDto createDto)
    {
        var command = new AddComplaintCommand(
            User.GetUserId(),
            createDto.Title,
            createDto.Text,
            createDto.CategoryId,
            createDto.Medias.GetMedia(),
            createDto.Captcha,
            createDto.Complaining,
            createDto.OrganizationId);
        var result = await Sender.Send(command);

        return Ok(result);
    }

    [HttpPost("Operate")]
    public async Task<IActionResult> Operate([FromForm] ComplaintOperateCitizenDto operateDto)
    {
        var command = new ReplyComplaintCitizenCommand(
            operateDto.TrackingNumber,
            operateDto.Text,
            operateDto.Medias.GetMedia(),
            operateDto.Operation,
            operateDto.Password);
        var result = await Sender.Send(command);

        return Ok(result);
    }

    [HttpPost("Get")]
    public async Task<ActionResult<ComplaintCitizenResponse>> GetComplaintCitizen([FromBody] ComplaintCitizenGetDto complaintDto)
    {
        var query = new GetComplaintCitizenQuery(complaintDto.TrackingNumber, complaintDto.Password);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("List")]
    public async Task<ActionResult<List<ComplaintListCitizenResponse>>> List(
        [FromQuery] PagingInfo pagingInfo,
        [FromQuery] ComplaintListFilters filters)
    {
        var userId = User.GetUserId();
        var query = new GetComplaintListCitizenQuery(pagingInfo, filters, userId);
        var result = await Sender.Send(query);
        return Ok(result);
    }
}
