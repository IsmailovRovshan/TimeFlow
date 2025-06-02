using Microsoft.Extensions.Configuration;
using MimeKit;
using Services.Abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
namespace Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");
        var smtpHost = emailSettings["SmtpHost"];
        var smtpPort = int.Parse(emailSettings["SmtpPort"]);
        var useSsl   = bool.Parse(emailSettings["UseSsl"]);
        var userName = emailSettings["UserName"];
        var password = emailSettings["Password"];
        var fromName = emailSettings["FromName"];
        var fromAddr = emailSettings["FromAddress"];

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, fromAddr));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        // Используем HTML-формат
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlBody
        };
        message.Body = bodyBuilder.ToMessageBody();

        using var smtp = new SmtpClient();
        // Подключаемся к SMTP-серверу
        await smtp.ConnectAsync(smtpHost, smtpPort, useSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(userName, password);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}
