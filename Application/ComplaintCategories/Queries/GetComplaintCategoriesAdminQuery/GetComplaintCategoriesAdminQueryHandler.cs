using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Queries.GetComplaintCategoriesAdminQuery;

internal class GetComplaintCategoriesAdminQueryHandler : IRequestHandler<GetComplaintCategoriesAdminQuery, List<ComplaintCategory>>
{
    private readonly IComplaintCategoryRepository _categoryRepository;

    public GetComplaintCategoriesAdminQueryHandler(IComplaintCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<ComplaintCategory>> Handle(GetComplaintCategoriesAdminQuery request, CancellationToken cancellationToken)
    {
        var result = await _categoryRepository.GetAsync();
        return result.ToList();
    }
}
