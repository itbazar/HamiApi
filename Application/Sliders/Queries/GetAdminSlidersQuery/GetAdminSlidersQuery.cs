using Domain.Models.Sliders;
using MediatR;

namespace Application.Sliders.Queries.GetAdminSlidersQuery;

public record GetAdminSlidersQuery() : IRequest<Result<List<Slider>>>;
