namespace Shop.WebAPI.Services.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
}