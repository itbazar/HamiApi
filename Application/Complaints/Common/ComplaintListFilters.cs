using Domain.Models.ComplaintAggregate;

namespace Application.Complaints.Common;

public record ComplaintListFilters(List<ComplaintState>? States, string? TrackingNumber);