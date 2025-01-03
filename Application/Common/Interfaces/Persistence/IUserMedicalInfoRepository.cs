using Domain.Models.ComplaintAggregate;
using Domain.Models.Hami;

namespace Application.Common.Interfaces.Persistence;

public interface IUserMedicalInfoRepository 
{
    public Task<Result<bool>> Insert(UserMedicalInfo info);

    public Task<Result<bool>> Update(UserMedicalInfo info);
}
