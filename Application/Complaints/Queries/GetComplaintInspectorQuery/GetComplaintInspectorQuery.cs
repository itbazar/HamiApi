﻿using Application.Complaints.Queries.Common;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintInspectorQuery;

public record GetComplaintInspectorQuery(string TrackingNumber, string Password) : IRequest<ComplaintResponse>;