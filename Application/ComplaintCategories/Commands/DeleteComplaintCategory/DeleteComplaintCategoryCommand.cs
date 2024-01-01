using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Commands.DeleteComplaintCategory;

public record DeleteComplaintCategoryCommand(Guid Id, bool IsDeleted) : IRequest<ComplaintCategory>;
