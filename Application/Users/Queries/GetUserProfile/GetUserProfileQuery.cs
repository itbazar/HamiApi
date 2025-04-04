﻿using Domain.Models.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetUserProfile;

public sealed record GetUserProfileQuery(
    string UserId,string mode="Other") : IRequest<Result<ApplicationUser>>;
