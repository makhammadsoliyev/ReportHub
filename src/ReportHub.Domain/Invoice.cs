using ReportHub.Domain.Common;

namespace ReportHub.Domain;

public class Invoice : SoftDeletableAuditableEntity
{
	public DateTime IssueDate { get; set; }

	public DateTime DueDate { get; set; }

	public int ItemsCount => InvoiceItems.Count;

	public decimal Price => InvoiceItems.Sum(t => t.Price);

	public string CurrencyCode { get; set; }

	public PaymentStatus PaymentStatus { get; set; }

	public int InvoiceNumber { get; set; }

	public Guid CustomerId { get; set; }

	public Customer Customer { get; set; }

	public Guid OrganizationId { get; set; }

	public Organization Organization { get; set; }

	public IList<InvoiceItem> InvoiceItems { get; set; } = [];
}
