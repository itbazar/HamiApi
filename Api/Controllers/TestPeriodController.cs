using Api.Abstractions;
using Api.Contracts.TestPeriodContract;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.TestPeriodApp.Queries.GetTestPeriodByIdQuery;
using Application.TestPeriods.Commands.AddTestPeriodCommand;
using Application.TestPeriods.Commands.DeleteTestPeriodCommand;
using Application.TestPeriods.Commands.UpdateTestPeriodCommand;
using Application.TestPeriods.Queries.GetTestPeriodQuery;
using Domain.Models.Hami;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize(Roles = "Admin")]
public class TestPeriodController : ApiController
{
    public TestPeriodController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    public async Task<ActionResult<List<TestPeriodListItemDto>>> GetTestPeriodList([FromQuery] PagingInfo pagingInfo)
    {
        var query = new GetTestPeriodQuery(pagingInfo);
        var result = await Sender.Send(query);
        if (result.IsFailed)
            return Problem(result.ToResult());

        Response.AddPaginationHeaders(result.Value.Meta);
        return Ok(result.Value.Adapt<List<TestPeriodListItemDto>>());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TestPeriod>> GetTestPeriod(Guid id)
    {
        var query = new GetTestPeriodByIdQuery(id);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost]
    public async Task<ActionResult> AddTestPeriod([FromForm] AddTestPeriodDto testPeriodDto)
    {
        var command = new AddTestPeriodCommand(
            testPeriodDto.TestType,
            testPeriodDto.PeriodName,
            testPeriodDto.StartDate,
            testPeriodDto.EndDate,
            testPeriodDto.Code);

        var result = await Sender.Send(command);
        return result.Match(
            s => CreatedAtAction(nameof(GetTestPeriod), new { id = s.Id }, s),
            f => Problem(f));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateTestPeriod(Guid id, [FromForm] UpdateTestPeriodDto testPeriodDto)
    {
        var command = new UpdateTestPeriodCommand(
            id,
            testPeriodDto.TestType,
            testPeriodDto.PeriodName,
            testPeriodDto.StartDate,
            testPeriodDto.EndDate,
            testPeriodDto.Code);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteTestPeriod(Guid id)
    {
        var command = new DeleteTestPeriodCommand(id);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }
}
