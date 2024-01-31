using Api.Abstractions;
using Api.Contracts.Complaint;
using Api.Contracts.ComplaintCategory;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.Complaints.Commands.AddComplaintCommand;
using Application.Complaints.Common;
using Application.Complaints.Queries.GetComplaintInspectorQuery;
using Application.Complaints.Queries.GetComplaintListQuery;
using Application.Complaints.Queries.GetPossibleStatesCountQuery;
using Infrastructure.Options;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;

namespace Api.Controllers;

[Authorize(Roles = "Inspector")]
public class InspectorController : ApiController
{
    private readonly StorageOptions _storageOptions;
    public InspectorController(ISender sender, IOptions<StorageOptions> storageOptions) : base(sender)
    {
        _storageOptions = storageOptions.Value;
    }

    [HttpGet("PossibleStates")]
    public async Task<ActionResult<List<PossibleStateDto>>> GetPossibleStates()
    {
        var query = new GetPossibleStatesCountQuery();
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s.Adapt<List<PossibleStateDto>>()),
            f => Problem(f));
    }

    [HttpGet("List")]
    public async Task<ActionResult<List<ComplaintListInspectorResponse>>> List(
        [FromQuery] PagingInfo pagingInfo,
        [FromQuery] ComplaintListFilters filters)
    {
        var query = new GetComplaintListInspectorQuery(pagingInfo, filters);
        var result = await Sender.Send(query);
        if (result.IsFailed)
            return Problem(result.ToResult());

        Response.AddPaginationHeaders(result.Value.Meta);
        return Ok(result.Value);
    }

    [HttpPost("Get")]
    public async Task<ActionResult<ComplaintInspectorResponse>> Get([FromBody] ComplaintInspectorGetDto complaintDto)
    {
        var query = new GetComplaintInspectorQuery(complaintDto.TrackingNumber, complaintDto.Password);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost("Operate")]
    public async Task<IActionResult> AddDetail([FromForm] ComplaintOperationInspectorDto operateDto)
    {
        var command = new ReplyComplaintInspectorCommand(
            operateDto.TrackingNumber,
            operateDto.Text,
            operateDto.Medias.GetMedia(_storageOptions),
            operateDto.Operation,
            operateDto.IsPublic,
            operateDto.EncodedKey);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }
}
