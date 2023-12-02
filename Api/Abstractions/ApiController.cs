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
}
