using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Commands.DeleteComplaintCategory;

internal class DeleteComplaintCategoryCommandHandler : IRequestHandler<DeleteComplaintCategoryCommand, ComplaintCategory>
{
    private readonly IComplaintCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteComplaintCategoryCommandHandler(IComplaintCategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ComplaintCategory> Handle(DeleteComplaintCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetSingleAsync(cc => cc.Id == request.Id);
        if (category is null)
            throw new Exception("Not found!");
        category.Delete(request.IsDeleted);
        _categoryRepository.Update(category);
        await _unitOfWork.SaveAsync();
        return category;
    }
}
