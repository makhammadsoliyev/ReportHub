using ReportHub.Domain.Common;

namespace ReportHub.Domain;

public class Item : SoftDeletableAuditableEntity
{
	public string Name { get; set; }

	public string Description { get; set; }

	public decimal Price { get; set; }

	public string CurrencyCode { get; set; }

	public Guid OrganizationId { get; set; }

	public Organization Organization { get; set; }
}
