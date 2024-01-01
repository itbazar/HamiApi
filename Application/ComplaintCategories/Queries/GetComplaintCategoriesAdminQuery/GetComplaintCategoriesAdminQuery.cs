using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Queries.GetComplaintCategoriesAdminQuery;

public record GetComplaintCategoriesAdminQuery() : IRequest<List<ComplaintCategory>>;
