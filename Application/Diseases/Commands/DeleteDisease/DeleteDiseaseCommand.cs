using MediatR;
using FluentResults;
using Domain.Models.Hami;

namespace Application.Diseases.Commands.DeleteDisease;
public record DeleteDiseaseCommand(Guid Id, bool IsDeleted) : IRequest<Result<Disease>>;
