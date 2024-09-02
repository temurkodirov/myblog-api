﻿using MyBlog.Service.Dtos.Notifications;
using MyBlog.Service.Interfaces.Notifications;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;


namespace MyBlog.Service.Services.Notifications;


public class MailSender : IMailSender
{
    private readonly string _myMail = "info.gettalim@gmail.com";
    private readonly string _password = "wpktxqwivdpyzdyw";
    public async Task<bool> SendAsync(EmailMessage message)
    {
        try
        {
            var mail = new MimeMessage();

            mail.From.Add(MailboxAddress.Parse(_myMail));
            mail.To.Add(MailboxAddress.Parse(message.Recipent));

            mail.Subject = message.Title;
            mail.Body = new TextPart(TextFormat.Html) { Text = message.Content };

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_myMail, _password);
                await smtp.SendAsync(mail);
                await smtp.DisconnectAsync(true);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
}
