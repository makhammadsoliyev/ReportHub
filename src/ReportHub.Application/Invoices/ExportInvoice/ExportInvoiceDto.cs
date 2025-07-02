namespace ReportHub.Application.Invoices.ExportInvoice;

public class ExportInvoiceDto
{
	public byte[] Content { get; set; }

	public string ContentType { get; set; }

	public string FileName { get; set; }
}
