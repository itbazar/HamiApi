﻿using Api.Abstractions;
using Api.Contracts.QuestionContract;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.QuestionApp.Queries.GetQuestionByIdQuery;
using Application.Questions.Commands.AddQuestionCommand;
using Application.Questions.Commands.DeleteQuestionCommand;
using Application.Questions.Commands.UpdateQuestionCommand;
using Application.Questions.Queries.GetQuestionByTestPeriodIdQuery;
using Application.Questions.Queries.GetQuestionQuery;
using Domain.Models.Hami;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


public class QuestionController : ApiController
{
    public QuestionController(ISender sender) : base(sender)
    {
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<QuestionListItemDto>>> GetQuestionList([FromQuery] PagingInfo pagingInfo)
    {
        var query = new GetQuestionQuery(pagingInfo);
        var result = await Sender.Send(query);
        if (result.IsFailed)
            return Problem(result.ToResult());

        Response.AddPaginationHeaders(result.Value.Meta);
        return Ok(result.Value.Adapt<List<QuestionListItemDto>>());
    }

    [Authorize(Roles = "Admin,Patient")]
    [HttpGet("{testPeriodId:guid}/Questions")]
    public async Task<ActionResult<List<QuestionListItemDto>>> GetTestQuestions(Guid testPeriodId)
    {
        var query = new GetQuestionByTestPeriodIdQuery(testPeriodId);
        var result = await Sender.Send(query);
        if (result.IsFailed)
            return Problem(result.ToResult());

        return Ok(result.Value.Adapt<List<QuestionListItemDto>>());
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Question>> GetQuestion(Guid id)
    {
        var query = new GetQuestionByIdQuery(id);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> AddQuestion([FromForm] AddQuestionDto questionDto)
    {
        var command = new AddQuestionCommand(
            questionDto.TestType,
            questionDto.QuestionText,
            questionDto.IsDeleted);

        var result = await Sender.Send(command);
        return result.Match(
            s => CreatedAtAction(nameof(GetQuestion), new { id = s.Id }, s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateQuestion(Guid id, [FromForm] UpdateQuestionDto questionDto)
    {
        var command = new UpdateQuestionCommand(
            id,
            questionDto.TestType,
            questionDto.QuestionText,
            questionDto.IsDeleted);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteQuestion(Guid id)
    {
        var command = new DeleteQuestionCommand(id);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }
}
