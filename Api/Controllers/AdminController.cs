﻿using Api.Abstractions;
using Api.Contracts.KeyManagement;
using Api.ExtensionMethods;
using Application.Setup.Commands.AddPublicKey;
using Application.Setup.Commands.ChangeInspectorKey;
using Application.Setup.Commands.EditChartNames;
using Application.Setup.Commands.GenerateKeyPair;
using Application.Setup.Commands.Init;
using Application.Setup.Queries.GetPublicKeys;
using Domain.Models.ComplaintAggregate.Encryption;
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
        return result.Match(
            s => File(Encoding.ASCII.GetBytes(s), "text/plain", "private.txt"),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GenerateKeyPair")]
    public async Task<ActionResult<AsymmetricKey>> GenerateKeyPair()
    {
        var command = new GenerateKeyPairCommand();
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetKeys")]
    public async Task<ActionResult<List<PublicKeyResponse>>> GetKeys()
    {
        var command = new GetPublicKeysQuery();
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("AddKey")]
    public async Task<ActionResult<bool>> AddKey([FromBody] AddPublicKeyDto addKeyDto)
    {
        var command = new AddPublicKeyCommand(addKeyDto.Title, addKeyDto.PublicKey);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("ChangeKey")]
    public async Task<ActionResult<ChangeInspectorKeyResponse>> ChangeKey([FromBody] ChangeInspectorKeyDto changeKeyDto)
    {
        var command = new ChangeInspectorKeyCommand(
            changeKeyDto.PrivateKey,
            changeKeyDto.PublicKeyId,
            changeKeyDto.IsPolling);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s), 
            f => Problem(f));
    }

    [Authorize(Roles = "PowerUser")]
    [HttpPost("EditChartNames")]
    public async Task<ActionResult> EditChartNames()
    {
        var command = new EditChartNamesCommand();
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

}
