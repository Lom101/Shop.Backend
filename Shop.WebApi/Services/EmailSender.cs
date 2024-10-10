using System.Net;
using System.Net.Mail;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Services;

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        // Реализация отправки email, например, с помощью SMTP.
        using var client = new SmtpClient("smtp.yandex.ru")
        {
            Port = 587,
            Credentials = new NetworkCredential("bashakirov@kpfu.ru", "fhujtqjxxxpbavwg"),
            EnableSsl = true,
        };

        await client.SendMailAsync(new MailMessage("bashakirov@kpfu.ru", email, subject, message)
        {
            IsBodyHtml = true
        });
    }
}
