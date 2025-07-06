using ReportHub.Domain;

namespace ReportHub.Application.Invoices.GetLogById;

public class GetLogByIdDto
{
	public Guid Id { get; set; }

	public Guid UserId { get; set; }

	public Guid InvoiceId { get; set; }

	public DateTime TimeStamp { get; set; }

	public LogStatus Status { get; set; }
}
