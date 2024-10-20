using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Shop.WebAPI.Config;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Services;

public class EmailSender : IEmailSender
{
    private readonly SmtpServerSettings _emailSettings;

    public EmailSender(IOptions<SmtpServerSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        using var client = new SmtpClient(_emailSettings.SmtpServer)
        {
            Port = _emailSettings.Port,
            Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
            EnableSsl = _emailSettings.EnableSsl,
        };

        await client.SendMailAsync(new MailMessage(_emailSettings.Username, email, subject, message)
        {
            IsBodyHtml = true
        });
    }
}