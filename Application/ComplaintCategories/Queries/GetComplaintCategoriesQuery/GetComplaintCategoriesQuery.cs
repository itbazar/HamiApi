using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Queries.GetComplaintCategoriesQuery;

public record GetComplaintCategoriesQuery() : IRequest<List<ComplaintCategory>>;
