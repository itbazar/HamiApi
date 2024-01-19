using Api.Abstractions;
using Api.Contracts.ComplaintOrganization;
using Api.ExtensionMethods;
using Application.ComplaintCategories.Commands.EditComplaintCategory;
using Application.ComplaintOrganizations.Commands.AddComplaintOrganization;
using Application.ComplaintOrganizations.Commands.DeleteComplaintOrganization;
using Application.ComplaintOrganizations.Queries.GetComplaintCategoriesAdminQuery;
using Application.ComplaintOrganizations.Queries.GetComplaintOrganizationByIdQuery;
using Domain.Models.ComplaintAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ComplaintOrganizationsController : ApiController
{
    public ComplaintOrganizationsController(ISender sender) : base(sender)
    {
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<ComplaintOrganization>>> GetAll()
    {
        var command = new GetComplaintOrganizationsAdminQuery();
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            () => Problem());
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ComplaintOrganization>> Get(Guid id)
    {
        var command = new GetComplaintOrganizationByIdQuery(id);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            () => Problem());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateComplaintOrganizationRequest createDto)
    {
        var command = new AddComplaintOrganizationCommand(createDto.Title, createDto.Description);
        var result = await Sender.Send(command);
        return result.Match(
            s => CreatedAtAction(nameof(Get), new { id = s.Id }, s),
            () => Problem());
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateComplaintOrganizationRequest createDto)
    {
        var command = new EditComplaintOrganizationCommand(id, createDto.Title, createDto.Description);
        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            () => Problem());
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, [FromBody] bool isDeleted)
    {
        var command = new DeleteComplaintOrganizationCommand(id, isDeleted);
        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            () => Problem());
    }
}
