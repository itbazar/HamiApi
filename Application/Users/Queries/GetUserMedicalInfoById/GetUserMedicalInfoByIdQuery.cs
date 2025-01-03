using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetUserMedicalInfoById;

public record GetUserMedicalInfoByIdQuery(string UserId) : IRequest<Result<UserMedicalInfo>>;