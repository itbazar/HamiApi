using Api.Abstractions;
using Application.Sliders.Commands.AddSliderCommand;
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
        return await Sender.Send(query);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Slider>> GetSlider(Guid id)
    {
        var query = new GetSliderByIdQuery(id);
        var result = await Sender.Send(query);
        return result;
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
        return CreatedAtAction(nameof(GetSlider), new { id = result.Id }, result);
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

        await Sender.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteSlider(Guid id)
    {
        var command = new DeleteSliderCommand(id);

        await Sender.Send(command);
        return NoContent();
    }
}

public record UpdateSliderDto(string? Title, IFormFile? Image, string? Url, string? Description);
public record AddSliderDto(string Title, IFormFile Image, string Url = "", string Description = "");