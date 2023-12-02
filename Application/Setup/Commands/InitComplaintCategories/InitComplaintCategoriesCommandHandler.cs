using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.Setup.Commands.InitComplaintCategories;

internal class InitComplaintCategoriesCommandHandler : IRequestHandler<InitComplaintCategoriesCommand, bool>
{
    private readonly IComplaintCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InitComplaintCategoriesCommandHandler(IComplaintCategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(InitComplaintCategoriesCommand request, CancellationToken cancellationToken)
    {
        var complaintCategories = await _categoryRepository.GetAsync();
        if (complaintCategories is not null && complaintCategories.Count() > 0) 
        {
            return false;
        }
        var titles = new List<string>()
        {
            "رشاء",
            "ارتشاء",
        };

        foreach(var title in titles)
        {
            _categoryRepository.Insert(ComplaintCategory.Create(title, ""));
        }

        await _unitOfWork.SaveAsync();
        return true;
    }
}
