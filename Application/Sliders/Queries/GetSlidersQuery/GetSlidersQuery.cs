using Domain.Models.Sliders;
using MediatR;

namespace Application.Sliders.Queries.GetSlidersQuery;

public record GetSlidersQuery() : IRequest<Result<List<Slider>>>;
