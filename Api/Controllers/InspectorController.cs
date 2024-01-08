using Api.Abstractions;
using Api.Contracts.Complaint;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.Complaints.Commands.AddComplaintCommand;
using Application.Complaints.Common;
using Application.Complaints.Queries.GetComplaintInspectorQuery;
using Application.Complaints.Queries.GetComplaintListQuery;
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
    public async Task<ActionResult<List<ComplaintListInspectorResponse>>> List(
        [FromQuery] PagingInfo pagingInfo,
        [FromQuery] ComplaintListFilters filters)
    {
        var query = new GetComplaintListInspectorQuery(pagingInfo, filters);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpPost("Get")]
    public async Task<ActionResult<ComplaintInspectorResponse>> Get([FromBody] ComplaintInspectorGetDto complaintDto)
    {
        var query = new GetComplaintInspectorQuery(complaintDto.TrackingNumber, complaintDto.Password);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpPost("Operate")]
    public async Task<IActionResult> AddDetail([FromForm] ComplaintOperationInspectorDto operateDto)
    {
        var command = new ReplyComplaintInspectorCommand(
            operateDto.TrackingNumber,
            operateDto.Text,
            operateDto.Medias.GetMedia(),
            operateDto.Operation,
            operateDto.IsPublic,
            operateDto.EncodedKey);
        var result = await Sender.Send(command);

        return Ok(result);
    }
}
