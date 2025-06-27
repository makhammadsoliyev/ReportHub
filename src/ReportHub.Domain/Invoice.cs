using ReportHub.Domain.Common;

namespace ReportHub.Domain;

public class Invoice : SoftDeletableAuditableEntity
{
	public DateTime IssueDate { get; set; }

	public DateTime DueDate { get; set; }

	public int ItemsCount { get; set; }

	public decimal Price { get; set; }

	public string CurrencyCode { get; set; }

	public PaymentStatus PaymentStatus { get; set; }

	public int InvoiceNumber { get; set; }

	public Guid CustomerId { get; set; }

	public Customer Customer { get; set; }

	public Guid OrganizationId { get; set; }

	public Organization Organization { get; set; }
}
