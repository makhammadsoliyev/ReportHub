using Microsoft.AspNetCore.Hosting;
using QuestPDF.Fluent;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Pdf;

public class PdfService(IWebHostEnvironment environment) : IPdfService
{
	public byte[] GeneratePdf(Invoice invoice)
	{
		var document = new InvoiceDocument(invoice, environment);

		return document.GeneratePdf();
	}
}
