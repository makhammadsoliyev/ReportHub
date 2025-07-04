using ReportHub.Domain;

namespace ReportHub.Application.ExportLogs.GetLogsList;

public class GetLogsListDto
{
	public Guid Id { get; set; }

	public Guid UserId { get; set; }

	public Guid InvoiceId { get; set; }

	public DateTime TimeStamp { get; set; }

	public LogStatus Status { get; set; }
}
