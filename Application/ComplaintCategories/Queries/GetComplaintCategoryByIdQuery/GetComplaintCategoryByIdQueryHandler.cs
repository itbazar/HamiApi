using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Queries.GetComplaintCategoryByIdQuery;

internal class GetComplaintCategoryByIdQueryHandler(IComplaintCategoryRepository categoryRepository) : IRequestHandler<GetComplaintCategoryByIdQuery, Result<ComplaintCategory>>
{
    public async Task<Result<ComplaintCategory>> Handle(GetComplaintCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await categoryRepository.GetSingleAsync(cc => cc.IsDeleted == false);
        if (result is null)
            return GenericErrors.NotFound;
        return result;
    }
}
