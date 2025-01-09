using Api.Abstractions;
using Api.Contracts.TestPeriodResultContract;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.TestPeriodResults.Queries.GetTestPeriodResultByIdQuery;
using Application.TestPeriodResults.Commands.AddTestPeriodResultCommand;
using Application.TestPeriodResults.Commands.DeleteTestPeriodResultCommand;
using Application.TestPeriodResults.Commands.UpdateTestPeriodResultCommand;
using Application.TestPeriodResults.Queries.GetTestPeriodResultQuery;
using Domain.Models.Hami;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize(Roles = "Admin")]
public class TestPeriodResultController : ApiController
{
    public TestPeriodResultController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    public async Task<ActionResult<List<TestPeriodResultListItemDto>>> GetTestPeriodResultList([FromQuery] PagingInfo pagingInfo)
    {
        var query = new GetTestPeriodResultQuery(pagingInfo);
        var result = await Sender.Send(query);
        if (result.IsFailed)
            return Problem(result.ToResult());

        Response.AddPaginationHeaders(result.Value.Meta);
        //return Ok(result.Value.Adapt<List<TestPeriodResultListItemDto>>());

        // مپ کردن مقادیر به TestPeriodResultListItemDto
        var dtoList = result.Value.Select(x => new TestPeriodResultListItemDto(
            x.Id,
            x.UserId,
            x.User?.UserName ?? "نامشخص", // نام کاربر
            x.TestType,
            x.TotalScore,
            x.TestPeriodId,
            x.TestPeriod?.PeriodName ?? "نامشخص", // نام دوره تست
            x.CreatedAt
        )).ToList();

        return Ok(dtoList);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TestPeriodResult>> GetTestPeriodResult(Guid id)
    {
        var query = new GetTestPeriodResultByIdQuery(id);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost]
    public async Task<ActionResult> AddTestPeriodResult([FromForm] AddTestPeriodResultDto testPeriodResultDto)
    {
        var command = new AddTestPeriodResultCommand(
            testPeriodResultDto.UserId,
            testPeriodResultDto.TestType,
            testPeriodResultDto.TotalScore,
            testPeriodResultDto.TestPeriodId);

        var result = await Sender.Send(command);
        return result.Match(
            s => CreatedAtAction(nameof(GetTestPeriodResult), new { id = s.Id }, s),
            f => Problem(f));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateTestPeriodResult(Guid id, [FromForm] UpdateTestPeriodResultDto testPeriodResultDto)
    {
        var command = new UpdateTestPeriodResultCommand(
            id,
            testPeriodResultDto.TotalScore);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteTestPeriodResult(Guid id)
    {
        var command = new DeleteTestPeriodResultCommand(id);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }
}
