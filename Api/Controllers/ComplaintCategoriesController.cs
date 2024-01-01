using Api.Abstractions;
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
        return await Sender.Send(command);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ComplaintCategory>> Get(Guid id)
    {
        var command = new GetComplaintCategoryByIdQuery(id);
        return await Sender.Send(command);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateComplaintCategoryRequest createDto)
    {
        var command = new AddComplaintCategoryCommand(createDto.Title, createDto.Description);
        var category = await Sender.Send(command);
        return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateComplaintCategoryRequest createDto)
    {
        var command = new EditComplaintCategoryCommand(id, createDto.Title, createDto.Description);
        await Sender.Send(command);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, [FromBody] bool isDeleted)
    {
        var command = new DeleteComplaintCategoryCommand(id, isDeleted);
        await Sender.Send(command);
        return NoContent();
    }
}


public record CreateComplaintCategoryRequest(string Title, string Description);
public record UpdateComplaintCategoryRequest(string? Title, string? Description);