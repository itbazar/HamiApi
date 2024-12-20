﻿using Application.Common.Interfaces.Persistence;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Mapster;

namespace Application.Complaints.Commands.ReplyComplaintCitizenCommand;

public class ReplyComplaintCitizenCommandHandler(IComplaintRepository complaintRepository) : IRequestHandler<ReplyComplaintCitizenCommand, Result<bool>>
{
public async Task<Result<bool>> Handle(ReplyComplaintCitizenCommand request, CancellationToken cancellationToken)
    {
        var result = await complaintRepository.GetAsync(request.TrackingNumber);
        if (result.IsFailed)
            return result.ToResult();
        
        var complaint = result.Value;

        var replyResult = complaint.ReplyCitizen(
                request.Text,
                request.Medias.Adapt<List<Media>>(),
                Actor.Citizen,
                request.Operation,
                ComplaintContentVisibility.Everyone,
                request.Password);
        if (replyResult.IsFailed)
            return replyResult;

        await complaintRepository.Update(complaint);

        return true;
    }
}
