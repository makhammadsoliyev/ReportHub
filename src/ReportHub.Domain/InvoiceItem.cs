using ReportHub.Domain.Common;

namespace ReportHub.Domain;

public class InvoiceItem : SoftDeletableAuditableEntity
{
	public Guid ItemId { get; set; }

	public Item Item { get; set; }

	public Guid InvoiceId { get; set; }

	public Invoice Invoice { get; set; }

	public int ItemsCount { get; set; }

	public decimal Price { get; set; }

	public string CurrencyCode { get; set; }

	public Guid OrganizationId { get; set; }

	public Organization Organization { get; set; }
}
