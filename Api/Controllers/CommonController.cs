using Api.Abstractions;
using Application.ComplaintCategories.Queries;
using Application.Setup.Commands.InitComplaintCategories;
using Domain.Models.ComplaintAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class CommonController : ApiController
{
    public CommonController(ISender sender) : base(sender)
    {
    }

    [HttpGet("Init")]
    public async Task<IActionResult> Init()
    {
        var command = new InitComplaintCategoriesCommand();
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpGet("Categories")]
    public async Task<ActionResult<List<ComplaintCategory>>> GetCategories()
    {
        var query = new GetComplaintCategoriesQuery();
        var result = await Sender.Send(query);
        return Ok(result);
    }
}
