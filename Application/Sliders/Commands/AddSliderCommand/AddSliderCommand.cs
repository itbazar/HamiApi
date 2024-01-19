using Domain.Models.Sliders;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Sliders.Commands.AddSliderCommand;

public record AddSliderCommand(
    string Title,
    IFormFile Image,
    string Url,
    string Description) : IRequest<Result<Slider>>;
