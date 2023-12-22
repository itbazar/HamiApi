using Api.Abstractions;
using Application.Common.Interfaces.Encryption;
using Application.Setup.Commands.GenerateKeyPair;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class AdminController : ApiController
{
    public AdminController(ISender sender) : base(sender)
    {
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GenerateKeyPair")]
    public async Task<AsymmetricKey> GenerateKeyPair()
    {
        var command = new GenerateKeyPairCommand();
        return await Sender.Send(command);
    }
}
