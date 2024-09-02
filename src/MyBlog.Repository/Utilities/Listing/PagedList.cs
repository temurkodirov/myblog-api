namespace MyBlog.Repository.Utilities.Listing;

public class PagedList<T> : List<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public string Order { get; private set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PagedList(List<T> items, int count, int pageNumber, int pageSize, string order)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Order = order;

        AddRange(items);
    }

    public static PagedList<T> ToPagedList(int pageNumber, int pageSize, string order, IEnumerable<T> source = null)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PagedList<T>(items, count, pageNumber, pageSize, order);
    }

    public static PagedList<T> ToPagedListFromQuery(int pageNumber, int pageSize, string order, IQueryable<T> source = null)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PagedList<T>(items, count, pageNumber, pageSize, order);
    }
    public PagedListData<T> ToPagedListData()
    {
        return new PagedListData<T>
        {
            Items = this,
            MetaData = new MetaData()
            {
                CurrentPage = CurrentPage,
                TotalCount = TotalCount,
                TotalPages = TotalPages,
                PageSize = PageSize,
                HasPrevious = HasPrevious,
                HasNext = HasNext,
                Order = Order
            }
        };
    }
}
