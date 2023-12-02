namespace Domain.Models.Common;

public class Media
{
    public Guid Id { get; set; }
    public byte[] Data { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public MediaType MediaType { get; set; }
}

public class MediaCitizen : Media
{
    public byte[] SymmetricCipher { get; set; } = null!;
}

public class MediaInspector : Media
{
    public byte[] AsymmetricCipher { get; set; } = null!;
}