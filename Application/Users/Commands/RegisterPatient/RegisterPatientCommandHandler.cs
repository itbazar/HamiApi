using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Complaints.Commands.AddComplaintCommand;
using Application.Complaints.Commands.Common;
using Application.Users.Commands.ApprovedRegisterPatient;
using Application.Users.Common;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Errors;

namespace Application.Users.Commands.RegisterPatient;

public class RegisterPatientCommandHandler(
    IUserRepository userRepository,
    IUserMedicalInfoRepository userMedicalInfoRepository,
    ITestPeriodRepository testPeriodRepository,
    ITestPeriodResultRepository testPeriodResultRepository) : IRequestHandler<RegisterPatientCommand, Result<AddPatientResult>>
{
    public async Task<Result<AddPatientResult>> Handle(
        RegisterPatientCommand request,
        CancellationToken cancellationToken)
    {
        // بررسی وجود کاربر با شماره تلفن
        var existingUser = await userRepository.FindByNameAsync(request.PhoneNumber);
        if (existingUser != null)
        {
            return AuthenticationErrors.UserAlreadyExists;
        }


        var user = new ApplicationUser
        {
            UserName = request.PhoneNumber,
            PhoneNumber = request.PhoneNumber,
            NationalId = request.NationalId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            Education = request.Education,
            City = request.City,
            RegistrationStatus = RegistrationStatus.Pending // وضعیت ثبت‌نام: در انتظار تأیید
        };


        // ذخیره اطلاعات کاربر
        var result = await userRepository.CreateAsync(user, "pP@" + user.PhoneNumber); // ایجاد رمز عبور

        if (!result.Succeeded)
        {
            return AuthenticationErrors.UserCreationFailed;
        }
        await userRepository.AddToRoleAsync(user, "Patient");

        var userMedicalInfoResult = UserMedicalInfo.Register(
           user.Id, 
           request.Organ,
           request.DiseaseType,
           request.PatientStatus,
           request.Stage,
           request.PathologyDiagnosis,
           request.InitialWeight,
           request.SleepDuration,
           request.AppetiteLevel,
           request.PhoneNumber
       );

        if (userMedicalInfoResult.IsFailed)
        {
            return userMedicalInfoResult.ToResult();
        }
        var temp = userMedicalInfoResult.Value;
        await userMedicalInfoRepository.Insert(temp);

        var gadTestPeriod = await testPeriodRepository.GetAsyncByCode(101);
        var testResult = TestPeriodResult.Create(
           user.Id,
           TestType.GAD,
           request.GADScore,
           gadTestPeriod.Value.Id);
        await testPeriodResultRepository.Insert(testResult);

        var mddTestPeriod = await testPeriodRepository.GetAsyncByCode(102);
        var testResult2 = TestPeriodResult.Create(
           user.Id,
           TestType.MDD,
           request.MDDScore,
           mddTestPeriod.Value.Id);

        await testPeriodResultRepository.Insert(testResult2);

        return new AddPatientResult(user.UserName, user.PhoneNumber);
    }
}
