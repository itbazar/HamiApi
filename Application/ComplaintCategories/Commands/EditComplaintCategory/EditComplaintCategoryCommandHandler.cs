using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Commands.EditComplaintCategory;

internal class EditComplaintCategoryCommandHandler : IRequestHandler<EditComplaintCategoryCommand, ComplaintCategory>
{
    private readonly IComplaintCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditComplaintCategoryCommandHandler(IComplaintCategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ComplaintCategory> Handle(EditComplaintCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetSingleAsync(cc => cc.Id == request.Id);
        if (category is null)
            throw new Exception("Not found!");
        category.Update(request.Title, request.Description);
        _categoryRepository.Update(category);
        await _unitOfWork.SaveAsync();
        return category;
    }
}
