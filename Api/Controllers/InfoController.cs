using Api.Abstractions;
using Application.Charts.Queries.GetInfoQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class InfoController : ApiController
{
    public InfoController(ISender sender) : base(sender)
    {
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<InfoModel>> GetInfo(Guid id)
    {
        var query = new GetInfoQuery(id);
        var result = await Sender.Send(query);
        return Ok(result);
    }
}
