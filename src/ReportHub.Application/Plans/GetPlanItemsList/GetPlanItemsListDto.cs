namespace ReportHub.Application.Plans.GetPlanItemsList;

public class GetPlanItemsListDto
{
	public Guid Id { get; set; }

	public Guid PlanId { get; set; }

	public Guid ItemId { get; set; }

	public int ItemsCount { get; set; }

	public decimal Price { get; set; }

	public string CurrencyCode { get; set; }
}
