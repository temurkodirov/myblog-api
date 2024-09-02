namespace MyBlog.Repository.Utilities.Listing;

public abstract class QueryStringParameters
{
    const int maxPageSize = 50; // Default is always 50
    public int PageNumber { get; set; } = 1; // Default is first page

    private int _pageSize = 10; // Default is always 10
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

    private string _order = "desc"; // Default is descending

    public string Order
    {
        get
        {
            return _order;
        }
        set
        {
            _order = string.IsNullOrWhiteSpace(value) ? _order : value;
        }
    }
}
