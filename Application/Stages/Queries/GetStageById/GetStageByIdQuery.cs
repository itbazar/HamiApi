
using Domain.Models.Hami;
using MediatR;

namespace Application.ComplaintCategories.Queries.GetComplaintCategoryByIdQuery;
public record GetStageByIdQuery(Guid Id) : IRequest<Result<Stage>>;