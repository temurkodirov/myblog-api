namespace MyBlog.Domain.Exceptions;

public class ExpiredException : Exception
{
    public ExpiredException(int statusCode, string message) : base(message)
    {
        this.StatusCode = statusCode;
    }

    public int StatusCode { get; set; }
}

