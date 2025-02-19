﻿using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintOrganizations.Queries.GetComplaintCategoriesAdminQuery;

public record GetComplaintOrganizationsAdminQuery() : IRequest<Result<List<ComplaintOrganization>>>;
