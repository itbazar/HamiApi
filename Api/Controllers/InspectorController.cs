using Api.Abstractions;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.Complaints.Commands.AddComplaintCommand;
using Application.Complaints.Commands.Common;
using Application.Complaints.Queries.Common;
using Application.Complaints.Queries.GetComplaintInspectorQuery;
using Application.Complaints.Queries.GetComplaintListQuery;
using Domain.Models.ComplaintAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize(Roles = "Inspector")]
public class InspectorController : ApiController
{
    public InspectorController(ISender sender) : base(sender)
    {
    }

    [HttpGet("List")]
    public async Task<ActionResult<List<Complaint>>> List(
        [FromQuery] PagingInfo pagingInfo,
        [FromQuery] ComplaintListFilters filters)
    {
        var query = new GetComplaintListQuery(pagingInfo, filters);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpPost("Get")]
    public async Task<ActionResult<Complaint>> Get([FromBody] ComplaintInspectorGetDto complaintDto)
    {
        var query = new GetComplaintInspectorQuery(complaintDto.TrackingNumber, complaintDto.Password);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpPost("Operate")]
    public async Task<IActionResult> AddDetail([FromForm] ComplaintOperationInspectorDto operateDto)
    {
        List<MediaRequest> data = new List<MediaRequest>();
        if (operateDto.Medias is not null)
        {
            foreach (var file in operateDto.Medias)
            {
                data.Add(file.GetMedia());
            }
        }

        var command = new ReplyComplaintInspectorCommand(
            operateDto.TrackingNumber,
            operateDto.Text,
            data,
            ComplaintOperation.AddDetails,
            operateDto.IsPublic,
            operateDto.EncodedKey);
        var result = await Sender.Send(command);

        return Ok(result);
    }
}

public record ComplaintOperationInspectorDto(
    string TrackingNumber,
    string Text,
    List<IFormFile>? Medias,
    bool IsPublic,
    string EncodedKey);
public record ComplaintInspectorGetDto(string TrackingNumber, string Password);