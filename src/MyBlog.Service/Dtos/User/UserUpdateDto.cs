using Microsoft.AspNetCore.Http;

namespace MyBlog.Service.Dtos.User;

public class UserUpdateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsMale { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
  
}
