using ReportHub.Domain;

namespace ReportHub.Application.Invoices.GetInvoiceById;

public class GetInvoiceByIdDto
{
	public Guid Id { get; set; }

	public DateTime IssueDate { get; set; }

	public DateTime DueDate { get; set; }

	public int ItemsCount { get; set; }

	public decimal Price { get; set; }

	public string CurrencyCode { get; set; }

	public PaymentStatus PaymentStatus { get; set; }

	public int InvoiceNumber { get; set; }

	public Guid CustomerId { get; set; }
}
