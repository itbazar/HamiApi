using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Commands.DeleteComplaintCategory;

internal class DeleteComplaintCategoryCommandHandler(IComplaintCategoryRepository categoryRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteComplaintCategoryCommand, Result<ComplaintCategory>>
{
    public async Task<Result<ComplaintCategory>> Handle(DeleteComplaintCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetSingleAsync(cc => cc.Id == request.Id);
        if (category is null)
            throw new Exception("Not found!");
        category.Delete(request.IsDeleted);
        categoryRepository.Update(category);
        await unitOfWork.SaveAsync();
        return category;
    }
}
