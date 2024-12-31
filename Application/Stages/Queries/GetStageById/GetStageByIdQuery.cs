
using Domain.Models.ComplaintAggregate;
using Domain.Models.StageAggregate;
using MediatR;

namespace Application.ComplaintCategories.Queries.GetComplaintCategoryByIdQuery;
public record GetStageByIdQuery(Guid Id) : IRequest<Result<Stage>>;