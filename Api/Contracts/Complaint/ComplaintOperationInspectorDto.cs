using Domain.Models.ComplaintAggregate;

namespace Api.Contracts.Complaint;

public record ComplaintOperationInspectorDto(
    string TrackingNumber,
    string Text,
    List<IFormFile>? Medias,
    ComplaintOperation Operation,
    bool IsPublic,
    string EncodedKey);
