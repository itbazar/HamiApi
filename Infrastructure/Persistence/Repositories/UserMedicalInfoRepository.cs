using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Domain.Models.ComplaintAggregate;
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
