using Api.Abstractions;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Complaints.Commands.AddComplaintCommand;
using Application.Complaints.Commands.ReplyComplaintCitizenCommand;
using Application.Complaints.Common;
using Application.Complaints.Queries.GetComplaintCitizenQuery;
using Application.Complaints.Queries.GetComplaintListQuery;
using Domain.Models.ComplaintAggregate;
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
            createDto.Captcha);
        var result = await Sender.Send(command);

        return Ok(result);
    }

    [HttpPost("Authorized")]
    public async Task<IActionResult> AddComplaintAuthorized([FromForm] ComplaintCreateDto createDto)
    {
        var command = new AddComplaintCommand(
            User.GetUserId(),
            createDto.Title,
            createDto.Text,
            createDto.CategoryId,
            createDto.Medias.GetMedia(),
            createDto.Captcha);
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
    public async Task<ActionResult<ComplaintResponse>> GetComplaintCitizen([FromBody] ComplaintCitizenGetDto complaintDto)
    {
        var query = new GetComplaintCitizenQuery(complaintDto.TrackingNumber, complaintDto.Password);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("List")]
    public async Task<ActionResult<List<ComplaintListResponse>>> List(
        [FromQuery] PagingInfo pagingInfo,
        [FromQuery] ComplaintListFilters filters)
    {
        var userId = User.GetUserId();
        var query = new GetComplaintListQuery(pagingInfo, filters, userId);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    public record ComplaintCreateDto(
        string Title,
        string Text,
        Guid CategoryId,
        List<IFormFile>? Medias,
        CaptchaValidateModel Captcha);
    public record ComplaintOperateCitizenDto(
        string TrackingNumber,
        string Text,
        List<IFormFile>? Medias,
        ComplaintOperation Operation,
        string Password);
    public record ComplaintCitizenGetDto(string TrackingNumber, string Password);
}
