using Application.Common.Interfaces.Persistence;
using Application.ComplaintOrganizations.Queries.GetComplaintOrganizationByIdQuery;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Queries.GetComplaintCategoryByIdQuery;

internal class GetComplaintCategoryByIdQueryHandler : IRequestHandler<GetComplaintCategoryByIdQuery, ComplaintCategory>
{
    private readonly IComplaintCategoryRepository _categoryRepository;

    public GetComplaintCategoryByIdQueryHandler(IComplaintCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ComplaintCategory> Handle(GetComplaintCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _categoryRepository.GetSingleAsync(cc => cc.IsDeleted == false);
        if (result is null)
            throw new Exception("Not found!");
        return result;
    }
}
