using Api.Abstractions;
using Api.Contracts.Disease;
using Api.ExtensionMethods;
using Application.ComplaintCategories.Queries.GetComplaintCategoryByIdQuery;
using Application.Diseases.Commands.AddDisease;
using Application.Diseases.Commands.DeleteDisease;
using Application.Diseases.Commands.EditDisease;
using Application.Diseases.Queries.GetDiseases;
using Domain.Models.DiseaseAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class DiseasesController : ApiController
{
    public DiseasesController(ISender sender) : base(sender)
    {
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<Disease>>> GetAll()
    {
        var command = new GetDiseasesQuery();
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Disease>> Get(Guid id)
    {
        var command = new GetDiseaseByIdQuery(id);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDiseaseRequest createDto)
    {
        var command = new AddDiseaseCommand(createDto.Title, createDto.Description);
        var result = await Sender.Send(command);
        return result.Match(
            s => CreatedAtAction(nameof(Get), new { id = s.Id }, s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDiseaseRequest updateDto)
    {
        var command = new EditDiseaseCommand(id, updateDto.Title, updateDto.Description);
        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, [FromBody] bool isDeleted)
    {
        var command = new DeleteDiseaseCommand(id, isDeleted);
        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }
}
