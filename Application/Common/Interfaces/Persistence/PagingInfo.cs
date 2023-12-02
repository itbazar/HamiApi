namespace Application.Common.Interfaces.Persistence;

public class PagingInfo
{
    const int maxPageSize = 50;
    private int _pageNumber = 1;
    public int PageNumber
    {
        get
        {
            return _pageNumber;
        }
        set
        {
            _pageNumber = value > 0 ? value : 1;
        }
    }
    private int _pageSize = 10;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = value > maxPageSize ? maxPageSize : value;
        }
    }

    //public List<QueryItem> Query { get; set; }
    public QueryItem? QueryItem { get; set; }
    public OrderBy? OrderBy { get; set; }
}

public class QueryItem
{
    public string Column { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class OrderBy
{
    public string Column { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
