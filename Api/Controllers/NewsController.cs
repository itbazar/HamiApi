using Api.Abstractions;
using Api.Contracts.NewsContract;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.NewsApp.Commands.AddNewsCommand;
using Application.NewsApp.Commands.DeleteNewsCommand;
using Application.NewsApp.Commands.UpdateNewsCommand;
using Application.NewsApp.Queries.GetAdminNewsQuery;
using Application.NewsApp.Queries.GetNewsByIdQuery;
using Domain.Models.News;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize(Roles = "Admin")]
public class NewsController : ApiController
{
    public NewsController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    public async Task<ActionResult<List<News>>> GetNewsList([FromQuery] PagingInfo pagingInfo)
    {
        var query = new GetAdminNewsQuery(pagingInfo);
        var result = await Sender.Send(query);
        if (result.IsFailed)
            return Problem(result.ToResult());

        Response.AddPaginationHeaders(result.Value.Meta);
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<News>> GetNews(Guid id)
    {
        var query = new GetNewsByIdQuery(id);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost]
    public async Task<ActionResult> AddNews([FromForm] AddNewsDto newsDto)
    {
        var command = new AddNewsCommand(
            newsDto.Title,
            newsDto.Image,
            newsDto.Url,
            newsDto.Description,
            newsDto.Content);

        var result = await Sender.Send(command);
        return result.Match(
            s => CreatedAtAction(nameof(GetNews), new { id = s.Id }, s),
            f => Problem(f));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateNews(Guid id, [FromForm] UpdateNewsDto newsDto)
    {
        var command = new UpdateNewsCommand(
            id,
            newsDto.Title,
            newsDto.Image,
            newsDto.Url,
            newsDto.Description);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteNews(Guid id)
    {
        var command = new DeleteNewsCommand(id);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }
}

