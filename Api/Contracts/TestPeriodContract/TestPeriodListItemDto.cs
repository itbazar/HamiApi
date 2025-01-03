using Domain.Models.Common;
using Domain.Models.Hami;

namespace Api.Contracts.TestPeriodContract;

public record TestPeriodListItemDto(
    Guid Id,
   TestType TestType,
        string PeriodName,
        DateTime StartDate,
        DateTime EndDate,
        int Code);