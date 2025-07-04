namespace ReportHub.Application.Invoices.ExportInvoicesReport;

public class ExportInvoicesReportDto
{
	public byte[] Content { get; set; }

	public string ContentType { get; set; }

	public string FileName { get; set; }
}
