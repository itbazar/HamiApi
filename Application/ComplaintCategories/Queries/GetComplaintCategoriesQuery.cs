using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Queries;

public record GetComplaintCategoriesQuery():IRequest<List<ComplaintCategory>>;
