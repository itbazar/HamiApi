using Api.Abstractions;
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
    private readonly long _maxFileSize;
    private readonly int _maxFileCount;
    private readonly List<string> _allowedExtensions;
    public CitizenController(ISender sender, IConfiguration configuration) : base(sender)
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

    [HttpPost]
    public async Task<IActionResult> AddComplaint([FromForm] ComplaintCreateDto createDto)
    {
        var command = new AddComplaintCommand(
            null,
            createDto.Title,
            createDto.Text,
            createDto.CategoryId,
            createDto.Medias.GetMedia(_maxFileSize, _maxFileCount, _allowedExtensions),
            createDto.Captcha,
            createDto.Complaining,
            createDto.OrganizationId);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            () => Problem());
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
            createDto.Medias.GetMedia(_maxFileSize, _maxFileCount, _allowedExtensions),
            createDto.Captcha,
            createDto.Complaining,
            createDto.OrganizationId);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            () => Problem());
    }

    [HttpPost("Operate")]
    public async Task<IActionResult> Operate([FromForm] ComplaintOperateCitizenDto operateDto)
    {
        var command = new ReplyComplaintCitizenCommand(
            operateDto.TrackingNumber,
            operateDto.Text,
            operateDto.Medias.GetMedia(_maxFileSize, _maxFileCount, _allowedExtensions),
            operateDto.Operation,
            operateDto.Password);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            () => Problem());
    }

    [HttpPost("Get")]
    public async Task<ActionResult<ComplaintCitizenResponse>> GetComplaintCitizen([FromBody] ComplaintCitizenGetDto complaintDto)
    {
        var query = new GetComplaintCitizenQuery(complaintDto.TrackingNumber, complaintDto.Password);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            () => Problem());
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
        return result.Match(
            s => Ok(s),
            () => Problem());
    }
}
