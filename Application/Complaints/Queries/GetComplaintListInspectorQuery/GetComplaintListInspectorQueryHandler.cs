﻿using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Mapster;
using MediatR;

namespace Application.Complaints.Queries.GetComplaintListQuery;

internal class GetComplaintListInspectorQueryHandler : 
    IRequestHandler<GetComplaintListInspectorQuery, Result<PagedList<ComplaintListInspectorResponse>>>
{
    private readonly IComplaintRepository _complaintRepository;

    public GetComplaintListInspectorQueryHandler(IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<Result<PagedList<ComplaintListInspectorResponse>>> Handle(
        GetComplaintListInspectorQuery request, CancellationToken cancellationToken)
    {
        var complaintList = await _complaintRepository.GetListInspectorAsync(
            request.pagingInfo,
            request.Filters);
        if (complaintList.IsFailed)
            return complaintList.ToResult();

        return complaintList.Value.Adapt<PagedList<ComplaintListInspectorResponse>>();
    }
}
