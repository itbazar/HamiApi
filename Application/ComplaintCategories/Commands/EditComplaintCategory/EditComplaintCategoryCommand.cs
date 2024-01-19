using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Commands.EditComplaintCategory;

public record EditComplaintCategoryCommand(Guid Id, string? Title, string? Description) : IRequest<Result<ComplaintCategory>>;
