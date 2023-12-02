using Api.Abstractions;
using Application.ComplaintCategories.Queries;
using Application.Complaints.Commands.AddComplaintCommand;
using Application.Complaints.Queries.GetComplaintCitizenQuery;
using Application.Setup.Commands.InitComplaintCategories;
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
    public async Task<IActionResult> AddComplaint(ComplaintCreateDto createDto)
    {
        var command = new AddComplaintCommand(createDto.Title, createDto.Text, createDto.CategoryId);
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

    [HttpGet]
    public async Task<ActionResult<Complaint>> GetComplaint([FromQuery] ComplaintCitizenGetDto complaintDto)
    {
        var query = new GetComplaintCitizenQuery(complaintDto.TrackingNumber, complaintDto.Password);
        var result = await Sender.Send(query);
        return Ok(result);
    }
    public record ComplaintCreateDto(string Title, string Text, Guid CategoryId);
    public record ComplaintCitizenGetDto(string TrackingNumber, string Password);
}
