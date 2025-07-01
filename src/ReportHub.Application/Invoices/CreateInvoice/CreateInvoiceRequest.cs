using ReportHub.Domain;

namespace ReportHub.Application.Invoices.CreateInvoice;

public class CreateInvoiceRequest
{
	public DateTime IssueDate { get; set; }

	public DateTime DueDate { get; set; }

	public PaymentStatus PaymentStatus { get; set; }

	public Guid CustomerId { get; set; }
}
