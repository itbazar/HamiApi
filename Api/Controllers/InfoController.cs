using Api.Abstractions;
using Api.ExtensionMethods;
using Application.Charts.Queries.GetChartByIdQuery;
using Application.Charts.Queries.GetChartsQuery;
using Application.Charts.Queries.GetInfoQuery;
using Domain.Models.ChartAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class InfoController : ApiController
{
    public InfoController(ISender sender) : base(sender)
    {
    }


    [Authorize]
    [HttpGet("List")]
    public async Task<ActionResult<List<ChartResponse>>> GetChartsList()
    {
        var query = new GetChartsQuery(User.GetUserRoles());
        return await Sender.Send(query);
    }

    [Authorize]
    [HttpGet("{code:int}")]
    public async Task<ActionResult<InfoModel>> GetInfo(int code)
    {
        var query = new GetInfoQuery(code);
        var result = await Sender.Send(query);
        return Ok(result);
    }
}
