namespace Application.Charts.Queries.GetInfoQuery;

public class InfoModel
{
    public List<InfoSingleton> Singletons { get; set; } = new List<InfoSingleton>();
    public List<InfoChart> Charts { get; set; } = new List<InfoChart>();
    public InfoModel Add(InfoSingleton singleton)
    {
        Singletons.Add(singleton);
        return this;
    }
    public InfoModel Add(InfoChart chart)
    {
        Charts.Add(chart);
        return this;
    }
    public InfoModel Add(InfoModel infoDto)
    {
        Singletons.AddRange(infoDto.Singletons);
        Charts.AddRange(infoDto.Charts);
        return this;
    }
}

public record InfoSingleton(string Value, string Title, string Icon);

public record InfoChart(string ChartTitle, string ChartIcon, bool IsStacked, bool IsHorizontal)
{
    public List<InfoSerie> Series { get; set; } = new List<InfoSerie>();
    public InfoChart Add(InfoSerie serie)
    {
        Series.Add(serie);
        return this;
    }

    public InfoChart Sort(int serieIndex = 0)
    {
        if (Series.Count < serieIndex)
            return this;

        var keys = Series[0].Values.Select(p => double.Parse(p.Value)).ToArray();
        for (int i = 0; i < Series.Count; i++)
        {
            var tmpKeys = (double[])keys.Clone();
            var tmp = Series[i].Values.ToArray();
            Array.Sort(tmpKeys, tmp);
            Series[i].Values = new List<DataItem>(tmp);
            Series[i].Values.Reverse();
        }
        return this;
    }
}

public record InfoSerie(string Title, string Icon)
{
    public List<DataItem> Values { get; set; } = new List<DataItem>();
    public InfoSerie Add(DataItem item)
    {
        Values.Add(item);
        return this;
    }

    public InfoSerie Add(List<DataItem> items)
    {
        Values.AddRange(items);
        return this;
    }

    public InfoSerie Add(string Title, string Value, string DisplayValue, object? Parameters = null)
    {
        Values.Add(new DataItem(Title, Value, DisplayValue, Parameters));
        return this;
    }
}

public record DataItem(string Title, string Value, string DisplayValue, object? Parameters = null);

