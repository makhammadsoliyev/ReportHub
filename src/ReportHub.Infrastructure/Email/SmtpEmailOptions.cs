namespace ReportHub.Infrastructure.Email;

public class SmtpEmailOptions
{
	public string Email { get; set; }

	public string Password { get; set; }

	public string Host { get; set; }

	public int Port { get; set; }
}
