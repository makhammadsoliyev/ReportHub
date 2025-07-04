namespace ReportHub.Application.Invoices.GetInvoicesRevenueCalculation;

public class GetInvoicesRevenueCalculationFilter
{
	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public Guid? CustomerId { get; set; }
}
