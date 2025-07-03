namespace ReportHub.Application.Plans.GetPlanById;

public class GetPlanByIdDto
{
	public Guid Id { get; set; }

	public string Title { get; set; }

	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }

	public string CurrencyCode { get; set; }

	public decimal Price { get; set; }

	public int ItemsCount { get; set; }
}
