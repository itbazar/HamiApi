using Domain.Models.Sliders;
using MediatR;

namespace Application.Sliders.Queries.GetSliderByIdQuery;

public record GetSliderByIdQuery(Guid Id) : IRequest<Result<Slider>>;
