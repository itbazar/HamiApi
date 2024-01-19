using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Queries.GetComplaintCategoriesQuery;

public record GetComplaintCategoriesQuery() : IRequest<Result<List<ComplaintCategory>>>;
