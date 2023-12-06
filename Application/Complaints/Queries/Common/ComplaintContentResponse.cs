using Domain.Models.ComplaintAggregate;

namespace Application.Complaints.Queries.Common;

public record ComplaintContentResponse(string Text, List<MediaResponse> Media, Actor Sender, DateTime DateTime);
