namespace Api.ExtensionMethods;

public static class TimeSpanExtensionMethods
{
    public static string ToPersianString(this TimeSpan value)
    {
        //return $"{value.Days} روز و {value.Hours}:{value.Minutes}";
        return $"{Math.Round(value.TotalHours * 100) / 100} ساعت";
    }

    public static string ToHoursValue(this TimeSpan value)
    {
        return $"{Math.Round(value.TotalHours * 100) / 100}";
    }
}
