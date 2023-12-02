using MediatR;

namespace Application.Setup.Commands.InitComplaintCategories;

public record InitComplaintCategoriesCommand():IRequest<bool>;
