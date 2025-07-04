namespace ReportHub.Application.Invoices.GetOverdueInvoicesAnalysis;

public class GetOverdueInvoicesAnalysisDto
{
	public int Count { get; set; }

	public decimal Amount { get; set; }

	public string CurrencyCode { get; set; }
}
