namespace MyBlog.Repository.Utilities.Listing;

public class MetaData
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public string Order { get; set; } = string.Empty;

    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }
}
