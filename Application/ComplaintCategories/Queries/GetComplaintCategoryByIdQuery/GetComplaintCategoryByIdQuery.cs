using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Queries.GetComplaintCategoryByIdQuery;

public record GetComplaintCategoryByIdQuery(Guid Id) : IRequest<ComplaintCategory>;
