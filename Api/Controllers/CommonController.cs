using Api.Abstractions;
using Api.ExtensionMethods;
using Application.Authentication.Queries.CaptchaQuery;
using Application.Common.Interfaces.Persistence;
using Application.Communications.Commands;
using Infrastructure.Options;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers;

public class CommonController : ApiController
{
    private readonly StorageOptions _storageOptions;
    public CommonController(ISender sender, IOptions<StorageOptions> storageOptions) : base(sender)
    {
        _storageOptions = storageOptions.Value;
    }


    [HttpGet("Captcha")]
    public async Task<ActionResult<string>> GetCaptcha()
    {
        var query = new CaptchaQuery();
        var result = await Sender.Send(query);
        if (result is null)
            throw new Exception();
        Response.Headers.Append("Captcha-Key", result.Value.Key.ToString());
        //return "data:image/jpg;base64," + Convert.ToBase64String(result.Data);
        return result.Match(
            s => File(s.Data, "image/jpg"),
            f => Problem(f));
    }

    [HttpGet("Options")]
    public ActionResult<OptionsGetDto> GetOptions()
    {
        return _storageOptions.Adapt<OptionsGetDto>();
    }

    [Authorize]
    [HttpPost("ConnectionId")]
    public async Task<ActionResult> AddConnectionId([FromBody] string connectionId)
    {
        var userId = User.GetUserId();
        var command = new AddSignalRConnectionIdCommand(userId, connectionId);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }
}

public record OptionsGetDto(string AllowedExtensions, int MaxFileCount, long MaxFileSize, int MaxTextLength);
