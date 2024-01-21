using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Abstractions;

[Route("api/[controller]")]
[ApiController]
public class ApiController : ControllerBase
{
    protected readonly ISender Sender;

    protected ApiController(ISender sender)
    {
        Sender = sender;
    }

    protected ActionResult Problem(Result result)
    {
        return Problem(
            String.Join("\r\n", result.Errors.Select(e => e.Message).ToList()),
            statusCode: StatusCodes.Status500InternalServerError);
    }
}
