using Application.Common.Interfaces.Persistence;
using Domain.Models.Sliders;

namespace Application.Sliders.Queries.GetAdminSlidersQuery;

public record GetAdminSlidersQuery(PagingInfo PagingInfo) : IRequest<Result<PagedList<Slider>>>;
