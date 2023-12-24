namespace Api.Contracts.Complaint;

public record ComplaintOperationInspectorDto(
    string TrackingNumber,
    string Text,
    List<IFormFile>? Medias,
    bool IsPublic,
    string EncodedKey);
