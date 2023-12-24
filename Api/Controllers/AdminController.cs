using Api.Abstractions;
using Api.Contracts.KeyManagement;
using Application.Common.Interfaces.Encryption;
using Application.Setup.Commands.AddPublicKey;
using Application.Setup.Commands.ChangeInspectorKey;
using Application.Setup.Commands.GenerateKeyPair;
using Application.Setup.Commands.Init;
using Application.Setup.Queries.GetPublicKeys;
using Domain.Models.PublicKeys;
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
    public async Task<ActionResult<AsymmetricKey>> GenerateKeyPair()
    {
        var command = new GenerateKeyPairCommand();
        return await Sender.Send(command);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetKeys")]
    public async Task<ActionResult<List<PublicKeyResponse>>> GetKeys()
    {
        var command = new GetPublicKeysQuery();
        return await Sender.Send(command);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("AddKey")]
    public async Task<ActionResult<bool>> AddKey([FromBody] AddPublicKeyDto addKeyDto)
    {
        var command = new AddPublicKeyCommand(addKeyDto.Title, addKeyDto.PublicKey);
        return await Sender.Send(command);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("ChangeKey")]
    public async Task<ActionResult<bool>> ChangeKey([FromBody] ChangeInspectorKeyDto changeKeyDto)
    {
        var command = new ChangeInspectorKeyCommand(changeKeyDto.PrivateKey, changeKeyDto.PublicKeyId);
        return await Sender.Send(command);
    }
}
