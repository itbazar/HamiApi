namespace Domain.ExtensionMethods;

public static class ByteArrayExtensionMethods
{
    public static string ToBase64(this byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }
    public static byte[] ParseBytes(this string base64String)
    {
        return Convert.FromBase64String(base64String);
    }
}
