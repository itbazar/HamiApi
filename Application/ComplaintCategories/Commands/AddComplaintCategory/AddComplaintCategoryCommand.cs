using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintCategories.Commands.AddComplaintCategory;

public record AddComplaintCategoryCommand(string Title, string Description) : IRequest<ComplaintCategory>;
