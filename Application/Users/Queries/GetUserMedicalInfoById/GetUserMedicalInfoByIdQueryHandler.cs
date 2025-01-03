using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetUserMedicalInfoById;

internal class GetUserMedicalInfoByIdQueryHandler(IUserRepository userRepository,
    IUserMedicalInfoRepository userMedicalInfoRepository) : IRequestHandler<GetUserMedicalInfoByIdQuery, Result<UserMedicalInfo>>
{
    public async Task<Result<UserMedicalInfo>> Handle(GetUserMedicalInfoByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(request.UserId);
        if (user is null)
            return UserErrors.UserNotExsists;

       var userMedicalInfo = await userMedicalInfoRepository.GetSingleAsync(cc => cc.UserId == request.UserId);
        if (userMedicalInfo is null)
            return UserErrors.UserNotExsists;

        return userMedicalInfo;
    }
}
