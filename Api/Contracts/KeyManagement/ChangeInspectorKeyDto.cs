namespace Api.Contracts.KeyManagement;

public record ChangeInspectorKeyDto(string PrivateKey, Guid PublicKeyId);