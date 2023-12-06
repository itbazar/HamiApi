using Api.Abstractions;
using Application.Common.Interfaces.Persistence;
using Application.ComplaintCategories.Queries;
using Application.Complaints.Commands.AddComplaintCommand;
using Application.Complaints.Commands.Common;
using Application.Complaints.Queries.GetComplaintCitizenQuery;
using Application.Complaints.Queries.GetComplaintInspectorQuery;
using Application.Complaints.Queries.GetComplaintListQuery;
using Application.Setup.Commands.InitComplaintCategories;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ComplaintController : ApiController
{
    public ComplaintController(ISender sender) : base(sender)
    {
    }

    [HttpPost]
    public async Task<IActionResult> AddComplaint([FromForm] ComplaintCreateDto createDto)
    {
        List<MediaRequest> data = new List<MediaRequest>();
        if(createDto.Medias is not null)
        {
            foreach (var file in createDto.Medias)
            {
                var tmp = new MemoryStream();
                file.CopyTo(tmp);
                var mediaRequest = new MediaRequest(file.FileName, MediaType.Image, tmp.ToArray());
                data.Add(mediaRequest);
            }
        }
            
        var command = new AddComplaintCommand(createDto.Title, createDto.Text, createDto.CategoryId, data);
        var result = await Sender.Send(command);

        return Ok(result);
    }

    [HttpPost("Reply")]
    public async Task<IActionResult> ReplyComplaintInspector([FromForm] ComplaintReplyDto replyDto)
    {
        List<MediaRequest> data = new List<MediaRequest>();
        if (replyDto.Medias is not null)
        {
            foreach (var file in replyDto.Medias)
            {
                var tmp = new MemoryStream();
                file.CopyTo(tmp);
                var mediaRequest = new MediaRequest(file.FileName, MediaType.Image, tmp.ToArray());
                data.Add(mediaRequest);
            }
        }

        var command = new ReplyComplaintInspectorCommand(replyDto.TrackingNumber, replyDto.Text, data, replyDto.EncryptedKey);
        var result = await Sender.Send(command);

        return Ok(result);
    }


    [HttpGet("Init")]
    public async Task<IActionResult> Init()
    {
        var command = new InitComplaintCategoriesCommand();
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpGet("Categories")]
    public async Task<ActionResult<List<ComplaintCategory>>> GetCategories()
    {
        var query = new GetComplaintCategoriesQuery();
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpGet("CitizenGet")]
    public async Task<ActionResult<Complaint>> GetComplaintCitizen([FromQuery] ComplaintCitizenGetDto complaintDto)
    {
        var query = new GetComplaintCitizenQuery(complaintDto.TrackingNumber, complaintDto.Password);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpGet("InspectorGet")]
    public async Task<ActionResult<Complaint>> GetComplaintInspector([FromQuery] ComplaintInspectorGetDto complaintDto)
    {
        var query = new GetComplaintInspectorQuery(complaintDto.TrackingNumber, complaintDto.Password);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpGet("InspectorList")]
    public async Task<ActionResult<List<Complaint>>> GetComplaintListInspector([FromQuery] PagingInfo pagingInfo)
    {
        var query = new GetComplaintListQuery(pagingInfo);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    public record ComplaintCreateDto(string Title, string Text, Guid CategoryId, List<IFormFile>? Medias);
    public record ComplaintReplyDto(string TrackingNumber, string Text, List<IFormFile>? Medias, string EncryptedKey);
    public record ComplaintCitizenGetDto(string TrackingNumber, string Password);
    public record ComplaintInspectorGetDto(string TrackingNumber, string Password);
}
