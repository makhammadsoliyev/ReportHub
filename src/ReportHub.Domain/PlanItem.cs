using ReportHub.Domain.Common;

namespace ReportHub.Domain;

public class PlanItem : SoftDeletableAuditableEntity
{
	public Guid PlanId { get; set; }

	public Plan Plan { get; set; }

	public Guid ItemId { get; set; }

	public Item Item { get; set; }

	public int ItemsCount { get; set; }

	public decimal Price { get; set; }

	public string CurrencyCode { get; set; }

	public Guid OrganizationId { get; set; }

	public Organization Organization { get; set; }
}
