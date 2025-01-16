﻿using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Application.CounselingSessions.Queries.GetMentorCounselingSessionsQuery;

public record GetSessionUsersQuery(Guid SessionId) : IRequest<Result<List<ApplicationUser>>>;