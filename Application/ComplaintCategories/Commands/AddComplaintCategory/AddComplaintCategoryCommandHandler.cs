using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Commands.AddComplaintCategory;

internal class AddComplaintCategoryCommandHandler : IRequestHandler<AddComplaintCategoryCommand, ComplaintCategory>
{
    private readonly IComplaintCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddComplaintCategoryCommandHandler(IComplaintCategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ComplaintCategory> Handle(AddComplaintCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = ComplaintCategory.Create(request.Title, request.Description);
        _categoryRepository.Insert(category);
        await _unitOfWork.SaveAsync();
        return category;
    }
}
