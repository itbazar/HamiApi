using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Users.Commands.ApprovedRegisterPatient;
using Application.Users.Common;
using Domain.Models.Common;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Errors;

namespace Application.Users.Commands.CreateMentor;

public class CreateMentorCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateMentorCommand, Result<AddPatientResult>>
{
    public async Task<Result<AddPatientResult>> Handle(
        CreateMentorCommand request,
        CancellationToken cancellationToken)
    {
        // بررسی وجود کاربر با شماره تلفن
        var existingUser = await userRepository.FindByNameAsync(request.PhoneNumber);
        if (existingUser is not null && existingUser.RegistrationStatus == RegistrationStatus.Approved)
        {
            return AuthenticationErrors.UserAlreadyExists;
        }

        if (existingUser is not null && existingUser.RegistrationStatus == RegistrationStatus.Pending)
        {
            return AuthenticationErrors.RegistrationNotApproved;
        }

        var user = new ApplicationUser
        {
            UserName = request.PhoneNumber,
            PhoneNumber = request.PhoneNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Title = request.Title,
            Email = request.Email,
            //DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            Education = request.Education,
            City = request.City,
            RegistrationStatus = RegistrationStatus.Pending // وضعیت ثبت‌نام: در انتظار تأیید
        };

        // ذخیره اطلاعات کاربر
        var result = await userRepository.CreateAsync(user, request.Password); // ایجاد رمز عبور

        if (!result.Succeeded)
        {
            return AuthenticationErrors.UserCreationFailed;
        }
        await userRepository.AddToRoleAsync(user, "Mentor");
        await unitOfWork.SaveAsync();


        return new AddPatientResult(user.UserName, user.PhoneNumber);
    }
}
