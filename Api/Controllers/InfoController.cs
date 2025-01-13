using Api.Abstractions;
using Api.ExtensionMethods;
using Application.Charts.Queries.GetChartByIdQuery;
using Application.Charts.Queries.GetChartsQuery;
using Application.Charts.Queries.GetInfoQuery;
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
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize]
    [HttpGet("{code:int}")]
    public async Task<ActionResult<InfoModel>> GetInfo(int code)
    {
        var userId = User.GetUserId();
        var userRole = User.IsInRole("Admin") ? "Admin" : "Mentor";

        var query = new GetInfoQuery(code, userId, userRole);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost("{code:int}")]
    public async Task<ActionResult<InfoModel>> GetInfo(int code, [FromBody] string userId)
    {
        if(code == 5 && string.IsNullOrEmpty(userId))
            userId = User.GetUserId();

        var userRole = User.IsInRole("Admin") ? "Admin" : "Mentor";
        var query = new GetInfoQuery(code, userId, userRole); // ارسال UserId به Query
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

}
