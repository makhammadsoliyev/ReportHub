using ReportHub.Domain;

namespace ReportHub.Application.Invoices.UpdateInvoice;

public class UpdateInvoiceRequest
{
	public Guid Id { get; set; }

	public DateTime IssueDate { get; set; }

	public DateTime DueDate { get; set; }

	public PaymentStatus PaymentStatus { get; set; }

	public Guid CustomerId { get; set; }
}
