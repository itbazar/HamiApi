using Domain.Models.Sliders;
using MediatR;

namespace Application.Sliders.Commands.DeleteSliderCommand;

public record DeleteSliderCommand(
    Guid Id,
    bool? IsDeleted = null) : IRequest<Slider>;
