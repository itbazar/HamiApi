using Api.Abstractions;
using Api.Contracts.AnswerContract;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.AnswerApp.Queries.GetAnswerByIdQuery;
using Application.Answers.Commands.AddAnswerCommand;
using Application.Answers.Commands.DeleteAnswerCommand;
using Application.Answers.Commands.UpdateAnswerCommand;
using Application.Answers.Queries.GetAnswerQuery;
using Domain.Models.Hami;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize(Roles = "Admin")]
public class AnswerController : ApiController
{
    public AnswerController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    public async Task<ActionResult<List<AnswerListItemDto>>> GetAnswerList([FromQuery] PagingInfo pagingInfo)
    {
        var query = new GetAnswerQuery(pagingInfo);
        var result = await Sender.Send(query);
        if (result.IsFailed)
            return Problem(result.ToResult());

        Response.AddPaginationHeaders(result.Value.Meta);
        return Ok(result.Value.Adapt<List<AnswerListItemDto>>());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Answer>> GetAnswer(Guid id)
    {
        var query = new GetAnswerByIdQuery(id);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost]
    public async Task<ActionResult> AddAnswer([FromForm] AddAnswerDto answerDto)
    {
        var command = new AddAnswerCommand(
            answerDto.UserId,
            answerDto.QuestionId,
            answerDto.AnswerValue.Value,
            answerDto.TestPeriodId);

        var result = await Sender.Send(command);
        return result.Match(
            s => CreatedAtAction(nameof(GetAnswer), new { id = s.Id }, s),
            f => Problem(f));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateAnswer(Guid id, [FromForm] UpdateAnswerDto answerDto)
    {
        var command = new UpdateAnswerCommand(
            id,
            answerDto.AnswerValue.Value);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAnswer(Guid id)
    {
        var command = new DeleteAnswerCommand(id);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }
}
