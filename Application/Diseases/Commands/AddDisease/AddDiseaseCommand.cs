using Domain.Models.DiseaseAggregate;
using FluentResults;
using MediatR;

namespace Application.Diseases.Commands.AddDisease;

public record AddDiseaseCommand(string Title, string Description) : IRequest<Result<Disease>>;
