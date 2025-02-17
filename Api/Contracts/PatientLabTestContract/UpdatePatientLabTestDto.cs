using Domain.Models.Hami;

namespace Api.Contracts.PatientLabTestContract;

public record UpdatePatientLabTestDto(
    string? UserId,        // شناسه کاربر (اختیاری)
    LabTestType? TestType, // نوع آزمایش (اختیاری)
    decimal? TestValue,    // مقدار جدید آزمایش (اختیاری)
    string? Unit          // واحد آزمایش (اختیاری)
);
