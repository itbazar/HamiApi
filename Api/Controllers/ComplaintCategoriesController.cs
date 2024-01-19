using Api.Abstractions;
using Api.Contracts.ComplaintCategory;
using Api.ExtensionMethods;
using Application.ComplaintCategories.Commands.AddComplaintCategory;
using Application.ComplaintCategories.Commands.DeleteComplaintCategory;
using Application.ComplaintCategories.Commands.EditComplaintCategory;
using Application.ComplaintCategories.Queries.GetComplaintCategoriesAdminQuery;
using Application.ComplaintCategories.Queries.GetComplaintCategoryByIdQuery;
using Domain.Models.ComplaintAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ComplaintCategoriesController : ApiController
{
    public ComplaintCategoriesController(ISender sender) : base(sender)
    {
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<ComplaintCategory>>> GetAll()
    {
        var command = new GetComplaintCategoriesAdminQuery();
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            () => Problem());
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ComplaintCategory>> Get(Guid id)
    {
        var command = new GetComplaintCategoryByIdQuery(id);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            () => Problem());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateComplaintCategoryRequest createDto)
    {
        var command = new AddComplaintCategoryCommand(createDto.Title, createDto.Description);
        var result = await Sender.Send(command);
        return result.Match(
            s => CreatedAtAction(nameof(Get), new { id = s.Id }, s),
            () => Problem());
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateComplaintCategoryRequest createDto)
    {
        var command = new EditComplaintCategoryCommand(id, createDto.Title, createDto.Description);
        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            () => Problem());
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, [FromBody] bool isDeleted)
    {
        var command = new DeleteComplaintCategoryCommand(id, isDeleted);
        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            () => Problem());
    }
}
