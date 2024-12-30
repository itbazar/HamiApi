using Domain.Models.ComplaintAggregate;
using Domain.Models.DiseaseAggregate;
using MediatR;

namespace Application.ComplaintCategories.Queries.GetComplaintCategoryByIdQuery;
public record GetDiseaseByIdQuery(Guid Id) : IRequest<Result<Disease>>;
