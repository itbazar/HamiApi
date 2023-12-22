using Api.Abstractions;
using Application.Common.Interfaces.Encryption;
using Application.Setup.Commands.GenerateKeyPair;
using Application.Setup.Commands.Init;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Api.Controllers;

public class AdminController : ApiController
{
    public AdminController(ISender sender) : base(sender)
    {
    }

    [HttpGet("Init")]
    public async Task<IActionResult> Init()
    {
        var command = new InitCommand();
        var result = await Sender.Send(command);
        return File(Encoding.ASCII.GetBytes(result), "text/plain", "private.txt");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GenerateKeyPair")]
    public async Task<AsymmetricKey> GenerateKeyPair()
    {
        var command = new GenerateKeyPairCommand();
        return await Sender.Send(command);
    }
}
