namespace MyBlog.Domain.Exceptions.Auth;

public class VerificationTooManyRequestsException : TooManyRequestException
{
    public VerificationTooManyRequestsException()
    {
        this.TitleMessage = "You have reached your limits";
    }
}

