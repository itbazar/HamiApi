
using Api.Abstractions;
using Api.Contracts.Stage;
using Api.ExtensionMethods;
using Application.ComplaintCategories.Queries.GetComplaintCategoryByIdQuery;
using Application.Stages.Commands.AddStage;
using Application.Stages.Commands.DeleteStage;
using Application.Stages.Commands.EditStage;
using Application.Stages.Queries.GetStages;
using Domain.Models.StageAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class StagesController : ApiController
{
    public StagesController(ISender sender) : base(sender)
    {
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    //[AllowAnonymous] // این روت بدون احراز هویت در دسترس است
    public async Task<ActionResult<List<Stage>>> GetAll()
    {
        var command = new GetStagesQuery();
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Stage>> Get(Guid id)
    {
        var command = new GetStageByIdQuery(id);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    //[AllowAnonymous] // این روت بدون احراز هویت در دسترس است
    public async Task<IActionResult> Create([FromBody] CreateStageRequest createDto)
    {
        var command = new AddStageCommand(createDto.Title, createDto.Description);
        var result = await Sender.Send(command);
        return result.Match(
            s => CreatedAtAction(nameof(Get), new { id = s.Id }, s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStageRequest updateDto)
    {
        var command = new EditStageCommand(id, updateDto.Title, updateDto.Description);
        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, [FromBody] bool isDeleted)
    {
        var command = new DeleteStageCommand(id, isDeleted);
        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }
}