using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Queries.GetComplaintCategoriesAdminQuery;

internal class GetComplaintCategoriesAdminQueryHandler(
    IComplaintCategoryRepository categoryRepository) 
    : IRequestHandler<GetComplaintCategoriesAdminQuery, Result<List<ComplaintCategory>>>
{
    public async Task<Result<List<ComplaintCategory>>> Handle(GetComplaintCategoriesAdminQuery request, CancellationToken cancellationToken)
    {
        var result = await categoryRepository.GetAsync();
        return result.ToList();
    }
}
