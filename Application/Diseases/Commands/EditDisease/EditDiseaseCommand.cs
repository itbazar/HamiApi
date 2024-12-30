namespace Application.Diseases.Commands.EditDisease;

using MediatR;
using FluentResults;
using Domain.Models.DiseaseAggregate;

public record EditDiseaseCommand(Guid Id, string Title, string Description) : IRequest<Result<Disease>>;

