using Api.Abstractions;
using Api.ExtensionMethods;
using Application.Complaints.Commands.AddComplaintCommand;
using Application.Complaints.Commands.Common;
using Application.Complaints.Commands.ReplyComplaintCitizenCommand;
using Application.Complaints.Queries.Common;
using Application.Complaints.Queries.GetComplaintCitizenQuery;
using Domain.Models.ComplaintAggregate;
using MediatR;
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
        List<MediaRequest> data = new List<MediaRequest>();
        if(createDto.Medias is not null)
        {
            foreach (var file in createDto.Medias)
            {
                data.Add(file.GetMedia());
            }
        }
            
        var command = new AddComplaintCommand(createDto.Title, createDto.Text, createDto.CategoryId, data);
        var result = await Sender.Send(command);

        return Ok(result);
    }

    [HttpPost("Operate")]
    public async Task<IActionResult> Operate([FromForm] ComplaintOperateCitizenDto opetateDto)
    {
        List<MediaRequest> data = new List<MediaRequest>();
        if (opetateDto.Medias is not null)
        {
            foreach (var file in opetateDto.Medias)
            {
                data.Add(file.GetMedia());
            }
        }

        var command = new ReplyComplaintCitizenCommand(opetateDto.TrackingNumber, opetateDto.Text, data, opetateDto.Operation, opetateDto.Password);
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

    
    public record ComplaintCreateDto(string Title, string Text, Guid CategoryId, List<IFormFile>? Medias);
    public record ComplaintOperateCitizenDto(
        string TrackingNumber,
        string Text,
        List<IFormFile>? Medias,
        ComplaintOperation Operation,
        string Password);
    public record ComplaintCitizenGetDto(string TrackingNumber, string Password);
}
