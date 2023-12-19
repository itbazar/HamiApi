namespace Api.ExtensionMethods;

public static class DateTimeExtensionMethods
{
    public static DateTimeOffset TrimToSeconds(this DateTimeOffset value)
    {
        return new DateTimeOffset(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, 0, value.Offset);
    }
}
