namespace ReportHub.Application.Common.Interfaces.Services;

public interface IEmailService
{
	Task SendAsync(string toEmail, string subject, string body, byte[] attachment = null);
}
