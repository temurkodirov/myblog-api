namespace MyBlog.Domain.Constants;

public static class TimeConstants
{
    public const int UTC = 5;

    public static DateTime Now()
        => DateTime.UtcNow.AddHours(UTC);
}
