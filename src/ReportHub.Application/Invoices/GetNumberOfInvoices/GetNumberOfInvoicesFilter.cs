namespace ReportHub.Application.Invoices.GetNumberOfInvoices;

public class GetNumberOfInvoicesFilter
{
	public Guid? CustomerId { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }
}
