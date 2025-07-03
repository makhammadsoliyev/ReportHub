using ReportHub.Domain.Common;

namespace ReportHub.Domain;

public class Plan : SoftDeletableAuditableEntity
{
	public string Title { get; set; }

	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }

	public string CurrencyCode { get; set; }

	public decimal Price => PlanItems.Sum(t => t.Price);

	public int ItemsCount => PlanItems.Count;

	public Guid OrganizationId { get; set; }

	public Organization Organization { get; set; }

	public IList<PlanItem> PlanItems { get; set; } = [];
}
