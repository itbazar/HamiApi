using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Queries.GetComplaintCategoriesQuery;

internal class GetComplaintCategoriesQueryHandler : IRequestHandler<GetComplaintCategoriesQuery, List<ComplaintCategory>>
{
    private readonly IComplaintCategoryRepository _categoryRepository;

    public GetComplaintCategoriesQueryHandler(IComplaintCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<ComplaintCategory>> Handle(GetComplaintCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _categoryRepository.GetAsync(cc => cc.IsDeleted == false);
        return result.ToList();
    }
}
