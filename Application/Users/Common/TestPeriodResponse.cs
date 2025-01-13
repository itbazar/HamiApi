using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;

namespace Application.Users.Common;

public record TestPeriodResponse(
   Guid Id,
   TestType TestType,
   string PeriodName,
   DateTime StartDate,
   DateTime EndDate,
   int Code,
   bool HasParticipated = false);