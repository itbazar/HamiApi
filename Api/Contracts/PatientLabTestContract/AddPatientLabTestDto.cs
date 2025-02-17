using Domain.Models.Hami;

namespace Api.Contracts.PatientLabTestContract;

public record AddPatientLabTestDto(
    string? UserId,        // شناسه کاربر (اختیاری، در صورتی که بیمار باشد نیازی نیست ارسال شود)
    LabTestType TestType,  // نوع آزمایش
    decimal TestValue,     // مقدار عددی آزمایش
    string Unit            // واحد آزمایش (مثلاً ng/ml)
);
