﻿using Domain.Models.ComplaintAggregate;
using MediatR;

namespace Application.ComplaintOrganizations.Queries.GetComplaintOrganizationQuery;

public record GetComplaintOrganizationsQuery() : IRequest<List<ComplaintOrganization>>;