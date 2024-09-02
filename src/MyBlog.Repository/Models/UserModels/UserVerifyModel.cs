namespace MyBlog.Repository.Models.UserModels;

public class UserVerifyModel
{
    public string Email { get; set; } = string.Empty;
    public int Code { get; set; }
}
