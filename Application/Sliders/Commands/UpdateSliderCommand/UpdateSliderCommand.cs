using Domain.Models.Sliders;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Sliders.Commands.UpdateSliderCommand;

public record UpdateSliderCommand(
    Guid Id,
    string? Title,
    IFormFile? Image,
    string? Url,
    string? Description) : IRequest<Result<Slider>>;
