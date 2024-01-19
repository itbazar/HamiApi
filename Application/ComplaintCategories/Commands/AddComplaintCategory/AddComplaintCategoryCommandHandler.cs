using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Commands.AddComplaintCategory;

internal class AddComplaintCategoryCommandHandler(IComplaintCategoryRepository categoryRepository, IUnitOfWork unitOfWork) : IRequestHandler<AddComplaintCategoryCommand, Result<ComplaintCategory>>
{
    public async Task<Result<ComplaintCategory>> Handle(AddComplaintCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = ComplaintCategory.Create(request.Title, request.Description);
        categoryRepository.Insert(category);
        await unitOfWork.SaveAsync();
        return category;
    }
}
