using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;

namespace Application.ComplaintCategories.Commands.EditComplaintCategory;

internal class EditComplaintCategoryCommandHandler(IComplaintCategoryRepository categoryRepository, IUnitOfWork unitOfWork) : IRequestHandler<EditComplaintCategoryCommand, Result<ComplaintCategory>>
{
    public async Task<Result<ComplaintCategory>> Handle(EditComplaintCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetSingleAsync(cc => cc.Id == request.Id);
        if (category is null)
            return GenericErrors.NotFound;
        category.Update(request.Title, request.Description);
        categoryRepository.Update(category);
        await unitOfWork.SaveAsync();
        return category;
    }
}
