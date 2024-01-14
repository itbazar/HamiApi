using Api.Abstractions;
using Api.Contracts.Complaint;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.Complaints.Commands.AddComplaintCommand;
using Application.Complaints.Common;
using Application.Complaints.Queries.GetComplaintInspectorQuery;
using Application.Complaints.Queries.GetComplaintListQuery;
using Domain.Models.ComplaintAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;

namespace Api.Controllers;

[Authorize(Roles = "Inspector")]
public class InspectorController : ApiController
{
    private readonly long _maxFileSize;
    private readonly int _maxFileCount;
    private readonly List<string> _allowedExtensions;
    public InspectorController(ISender sender, IConfiguration configuration) : base(sender)
    {
        _maxFileSize = configuration
            .GetSection("General")
            .GetSection("Files")
            .GetSection("MaxFileSize")
            .Get<long>();
        _maxFileCount = configuration
            .GetSection("General")
            .GetSection("Files")
            .GetSection("MaxFileCount")
            .Get<int>();
        var allowedExtensions = configuration
            .GetSection("General")
            .GetSection("Files")
            .GetSection("AllowedExtensions")
            .Get<string>();
        _allowedExtensions = allowedExtensions?.Split(',').ToList() ??
            new List<string> { "jpg", "png", "doc", "docx", "mp3", "avi", "mp4" };
        _allowedExtensions = _allowedExtensions.Select(x => x.Trim().ToUpper()).ToList();
    }

    [HttpGet("PossibleStates")]
    public ActionResult<List<PossibleStateDto>> GetPossibleStates()
    {
        var result = new List<PossibleStateDto>();
        foreach(var item in Enum.GetValues(typeof(ComplaintState)))
        {
            result.Add(new PossibleStateDto(((ComplaintState)item).GetDescription() ?? "", (int)item));
        }
        return result;
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
            operateDto.Medias.GetMedia(_maxFileSize, _maxFileCount, _allowedExtensions),
            operateDto.Operation,
            operateDto.IsPublic,
            operateDto.EncodedKey);
        var result = await Sender.Send(command);

        return Ok(result);
    }
}

public record PossibleStateDto(string Title, int Value);