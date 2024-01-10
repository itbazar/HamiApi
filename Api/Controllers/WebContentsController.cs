using Api.Abstractions;
using Api.Contracts.Sliders;
using Api.Contracts.WebContents;
using Application.Sliders.Commands.AddSliderCommand;
using Application.Sliders.Commands.DeleteSliderCommand;
using Application.Sliders.Commands.UpdateSliderCommand;
using Application.Sliders.Queries.GetAdminSlidersQuery;
using Application.Sliders.Queries.GetSliderByIdQuery;
using Application.WebContents.Commands.AddWebContent;
using Application.WebContents.Commands.EditWebContent;
using Application.WebContents.Queries.GetAdminWebContentsQuery;
using Application.WebContents.Queries.GetWebContentByIdQuery;
using Domain.Models.Sliders;
using Domain.Models.WebContents;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize(Roles = "Admin")]
public class WebContentsController : ApiController
{
    public WebContentsController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    public async Task<ActionResult<List<WebContent>>> GetWebContentsList()
    {
        var query = new GetAdminWebContentsQuery();
        return await Sender.Send(query);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WebContent>> GetWebContentById(Guid id)
    {
        var query = new GetWebContentByIdQuery(id);
        var result = await Sender.Send(query);
        return result;
    }

    [HttpPost]
    public async Task<ActionResult> AddWebContent([FromForm] AddWebContentDto webContentDto)
    {
        var command = new AddWebContentCommand(
            webContentDto.Title,
            webContentDto.Description,
            webContentDto.Content);

        var result = await Sender.Send(command);
        return CreatedAtAction(nameof(GetWebContentById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateSlider(Guid id, [FromForm] UpdateWebContentDto webContentDto)
    {
        var command = new EditWebContentCommand(
            id,
            webContentDto.Title,
            webContentDto.Description,
            webContentDto.Content);

        await Sender.Send(command);
        return NoContent();
    }
}

