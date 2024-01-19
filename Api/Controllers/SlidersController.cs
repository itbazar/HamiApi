﻿using Api.Abstractions;
using Api.Contracts.Sliders;
using Api.ExtensionMethods;
using Application.Sliders.Commands.AddSliderCommand;
using Application.Sliders.Commands.DeleteSliderCommand;
using Application.Sliders.Commands.UpdateSliderCommand;
using Application.Sliders.Queries.GetAdminSlidersQuery;
using Application.Sliders.Queries.GetSliderByIdQuery;
using Domain.Models.Sliders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize(Roles = "Admin")]
public class SlidersController : ApiController
{
    public SlidersController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    public async Task<ActionResult<List<Slider>>> GetSlidersList()
    {
        var query = new GetAdminSlidersQuery();
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            () => Problem());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Slider>> GetSlider(Guid id)
    {
        var query = new GetSliderByIdQuery(id);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            () => Problem());
    }

    [HttpPost]
    public async Task<ActionResult> AddSlider([FromForm] AddSliderDto sliderDto)
    {
        var command = new AddSliderCommand(
            sliderDto.Title,
            sliderDto.Image,
            sliderDto.Url,
            sliderDto.Description);

        var result = await Sender.Send(command);
        return result.Match(
            s => CreatedAtAction(nameof(GetSlider), new { id = s.Id }, s),
            () => Problem());
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateSlider(Guid id, [FromForm] UpdateSliderDto sliderDto)
    {
        var command = new UpdateSliderCommand(
            id,
            sliderDto.Title,
            sliderDto.Image,
            sliderDto.Url,
            sliderDto.Description);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(), 
            () => Problem());
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteSlider(Guid id)
    {
        var command = new DeleteSliderCommand(id);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            () => Problem());
    }
}

