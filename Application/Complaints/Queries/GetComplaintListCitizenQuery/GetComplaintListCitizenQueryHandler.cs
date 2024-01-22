﻿using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Mapster;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintListQuery;

internal class GetComplaintListCitizenQueryHandler : 
    IRequestHandler<GetComplaintListCitizenQuery, Result<List<ComplaintListCitizenResponse>>>
{
    private readonly IComplaintRepository _complaintRepository;

    public GetComplaintListCitizenQueryHandler(IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<Result<List<ComplaintListCitizenResponse>>> Handle(
        GetComplaintListCitizenQuery request, CancellationToken cancellationToken)
    {
        var complaintList = await _complaintRepository.GetListInspectorAsync(
            request.pagingInfo,
            request.Filters);
        if (complaintList.IsFailed)
            return complaintList.ToResult();

        return complaintList.Value.Adapt<List<ComplaintListCitizenResponse>>();
    }
}
