using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Queries.GetComplaintCategoriesQuery;

internal class GetComplaintCategoriesQueryHandler(
    IComplaintCategoryRepository categoryRepository) 
    : IRequestHandler<GetComplaintCategoriesQuery, Result<List<ComplaintCategory>>>
{
    public async Task<Result<List<ComplaintCategory>>> Handle(GetComplaintCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await categoryRepository.GetAsync(cc => cc.IsDeleted == false);
        return result.ToList();
    }
}
