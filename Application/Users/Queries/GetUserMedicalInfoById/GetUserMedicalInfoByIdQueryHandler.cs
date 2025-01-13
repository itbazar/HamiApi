using Application.Common.Interfaces.Persistence;
using Application.Users.Common;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using MediatR;
using System.ComponentModel;

namespace Application.Users.Queries.GetUserMedicalInfoById;

internal class GetUserMedicalInfoByIdQueryHandler(IUserRepository userRepository,
    IUserMedicalInfoRepository userMedicalInfoRepository,
    ITestPeriodResultRepository testPeriodResultRepository) : IRequestHandler<GetUserMedicalInfoByIdQuery, Result<UserMedicalInfoResponse>>
{
    public async Task<Result<UserMedicalInfoResponse>> Handle(GetUserMedicalInfoByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(request.UserId);
        if (user is null)
            return UserErrors.UserNotExsists;

        var userMedicalInfo = await userMedicalInfoRepository.GetSingleAsync(cc => cc.UserId == request.UserId);
        if (userMedicalInfo is null)
            return UserErrors.UserNotExsists;

        var testResultG = await testPeriodResultRepository.GetSingleAsync(q => q.UserId == request.UserId && q.TestType == TestType.GAD);
        var testResultM = await testPeriodResultRepository.GetSingleAsync(q => q.UserId == request.UserId && q.TestType == TestType.MDD);

        var response = new UserMedicalInfoResponse
        {
            UserName = user.UserName,
            //GroupName = user.UserGroupMemberships.Select(ugm => ugm.PatientGroup.Description) // گرفتن نام گروه از PatientGroup
               //.FirstOrDefault() ?? "بدون گروه",
            Organ = GetDescription(userMedicalInfo.Organ),
            DiseaseType = GetDescription(userMedicalInfo.DiseaseType),
            PatientStatus = GetDescription(userMedicalInfo.PatientStatus),
            Stage = userMedicalInfo.Stage,
            PathologyDiagnosis = userMedicalInfo.PathologyDiagnosis,
            InitialWeight = userMedicalInfo.InitialWeight,
            SleepDuration = userMedicalInfo.SleepDuration,
            AppetiteLevel = GetDescription(userMedicalInfo.AppetiteLevel),
            GadScore = testResultG.TotalScore,
            MddScore = testResultM.TotalScore
        };

        return response;
    }

    public static string GetDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .FirstOrDefault() as DescriptionAttribute;

        return attribute?.Description ?? value.ToString();
    }

}
