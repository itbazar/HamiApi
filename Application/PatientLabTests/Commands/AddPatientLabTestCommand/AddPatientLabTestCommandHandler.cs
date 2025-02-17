using Application.Common.Interfaces.Persistence;
using Application.PatientLabTests.Commands.AddPatientLabTestCommand;
using Domain.Models.Hami;
using FluentResults;
using MediatR;

internal class AddPatientLabTestCommandHandler(
    IUserRepository userRepository,
    IPatientLabTestRepository patientLabTestRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddPatientLabTestCommand, Result<PatientLabTest>>
{

    public async Task<Result<PatientLabTest>> Handle(AddPatientLabTestCommand request, CancellationToken cancellationToken)
    {
        // دریافت اطلاعات کاربر از پروفایل برای تعیین دسته‌بندی
        var user = await userRepository.FindByIdAsync(request.UserId);
        if (user == null)
            return UserErrors.UserNotExsists;

        // تعیین دسته‌بندی بر اساس سن و سیگاری بودن
        var userAge = DateTime.UtcNow.Year - user.DateOfBirth.Year;
        var isSmoker = user.IsSmoker;

        string unit;
        LabTestCategory category;

        switch (request.TestType)
        {
            case LabTestType.PSA:
                unit = "ng/ml";
                if (userAge >= 40 && userAge <= 49)
                    category = LabTestCategory.PSA_40_49;
                else if (userAge >= 50 && userAge <= 59)
                    category = LabTestCategory.PSA_50_59;
                else if (userAge >= 60 && userAge <= 69)
                    category = LabTestCategory.PSA_60_69;
                else
                    category = LabTestCategory.PSA_70_79;
                break;

            case LabTestType.CEA:
                unit = "mcg/l";
                category = isSmoker ? LabTestCategory.CEA_Smoker : LabTestCategory.CEA_NonSmoker;
                break;

            case LabTestType.CA125:
                unit = "U/ml";
                category = LabTestCategory.CA125;
                break;

            case LabTestType.CA15_3:
                unit = "U/ml";
                category = LabTestCategory.CA15_3;
                break;

            case LabTestType.CA27_29:
                unit = "U/ml";
                category = LabTestCategory.CA27_29;
                break;

            default:
                return PatientLabTestErrors.NotFound;
        }

        // ایجاد رکورد جدید
        var labTest = PatientLabTest.Create(
            request.UserId,
            request.TestType,
            request.TestValue,
            unit
        );

        patientLabTestRepository.Insert(labTest);
        await unitOfWork.SaveAsync();
        return labTest;
    }
}
