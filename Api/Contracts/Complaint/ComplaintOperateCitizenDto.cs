using Domain.Models.ComplaintAggregate;

namespace Api.Contracts.Complaint;

public record ComplaintOperateCitizenDto(
    string TrackingNumber,
    string Text,
    List<IFormFile>? Medias,
    ComplaintOperation Operation,
    string Password);
