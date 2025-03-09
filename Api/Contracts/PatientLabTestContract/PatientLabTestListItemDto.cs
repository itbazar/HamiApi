using Domain.Models.Hami;

namespace Api.Contracts.PatientLabTestContract;

public record PatientLabTestListItemDto(
    Guid Id,             // شناسه آزمایش
    string UserId,       // شناسه کاربر
    string Username,     // نام کاربر
    LabTestType TestType, // نوع آزمایش
    decimal TestValue,   // مقدار عددی آزمایش
    string Unit,         // واحد آزمایش
    DateTime CreatedAt,   // زمان ثبت آزمایش
    DateTime TestDate   // زمان انجام آزمایش
);
