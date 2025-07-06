namespace ReportHub.Application.Invoices.GetInvoicesRevenueCalculation;

public class GetInvoicesRevenueCalculationDto
{
	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public Guid? CustomerId { get; set; }

	public decimal Amount { get; set; }

	public string CurrencyCode { get; set; }
}
