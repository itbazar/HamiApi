using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Errors;
using System.Linq.Expressions;
using System.Linq;

namespace Infrastructure.Persistence.Repositories;

public class UserMedicalInfoRepository : GenericRepository<UserMedicalInfo>, IUserMedicalInfoRepository
{
    public UserMedicalInfoRepository(ApplicationDbContext context) : base(context)
    {
    }
}
