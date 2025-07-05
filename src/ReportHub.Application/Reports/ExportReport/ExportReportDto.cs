namespace ReportHub.Application.Reports.ExportReport;

public class ExportReportDto
{
	public byte[] Content { get; set; }

	public string ContentType { get; set; }

	public string FileName { get; set; }
}
