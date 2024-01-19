using Api.Abstractions;
using Api.Contracts.WebContents;
using Api.ExtensionMethods;
using Application.WebContents.Commands.AddWebContent;
using Application.WebContents.Commands.EditWebContent;
using Application.WebContents.Queries.GetAdminWebContentsQuery;
using Application.WebContents.Queries.GetWebContentByIdQuery;
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
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            () => Problem());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WebContent>> GetWebContentById(Guid id)
    {
        var query = new GetWebContentByIdQuery(id);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            () => Problem());
    }

    [HttpPost]
    public async Task<ActionResult> AddWebContent([FromForm] AddWebContentDto webContentDto)
    {
        var command = new AddWebContentCommand(
            webContentDto.Title,
            webContentDto.Description,
            webContentDto.Content);

        var result = await Sender.Send(command);
        return result.Match(
            s => CreatedAtAction(nameof(GetWebContentById), new { id = s.Id }, s),
            () => Problem());
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateSlider(Guid id, [FromForm] UpdateWebContentDto webContentDto)
    {
        var command = new EditWebContentCommand(
            id,
            webContentDto.Title,
            webContentDto.Description,
            webContentDto.Content);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(), 
            () => Problem());
    }
}

