using MyBlog.Service.Dtos.Notifications;

namespace MyBlog.Service.Interfaces.Notifications;

public interface IMailSender
{
    public Task<bool> SendAsync(EmailMessage message);

}
