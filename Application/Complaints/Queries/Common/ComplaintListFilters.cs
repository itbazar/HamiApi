using Domain.Models.ComplaintAggregate;

namespace Application.Complaints.Queries.Common;

public record ComplaintListFilters(List<ComplaintState>? States, string? TrackingNumber);