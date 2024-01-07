using Domain.Models.Sliders;
using MediatR;

namespace Application.Sliders.Commands.UpdateSliderCommand;

public record DeleteSliderCommand(
    Guid Id,
    bool? IsDeleted = null) : IRequest<Slider>;
