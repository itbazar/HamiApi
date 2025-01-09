using Domain.Models.Common;
using Domain.Models.Hami;

namespace Api.Contracts.TestPeriodResultContract;

public record TestPeriodResultListItemDto(
    Guid Id,
    string UserId,
    string Username, // اضافه کردن نام کاربر
    TestType TestType,
    int TotalScore,
    Guid TestPeriodId,
    string TestPeriodName, // اضافه کردن نام دوره تست
    DateTime CreatedAt);