using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ReportHub.Application.Common.Interfaces.Services;

namespace ReportHub.Infrastructure.Email;

public class EmailService(IOptions<SmtpEmailOptions> options) : IEmailService
{
	private readonly SmtpEmailOptions options = options.Value;

	public async Task SendAsync(string toEmail, string subject, string body, byte[] attachment = null)
	{
		var message = new MimeMessage();
		message.From.Add(new MailboxAddress("Umidbek Maxammadsoliyev", options.Email));
		message.To.Add(new MailboxAddress("Dear User!", toEmail));
		message.Subject = subject;

		var messageBody = new BodyBuilder { HtmlBody = body };
		if (attachment is not null)
		{
			messageBody.Attachments.Add("report.xls", attachment);
		}

		message.Body = messageBody.ToMessageBody();

		using var smtp = new SmtpClient();

		await smtp.ConnectAsync(options.Host, options.Port, SecureSocketOptions.StartTls);
		await smtp.AuthenticateAsync(options.Email, options.Password);
		await smtp.SendAsync(message);
		await smtp.DisconnectAsync(true);
	}
}
