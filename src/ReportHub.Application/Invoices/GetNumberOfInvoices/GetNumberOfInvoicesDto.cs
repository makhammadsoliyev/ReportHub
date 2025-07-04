namespace ReportHub.Application.Invoices.GetNumberOfInvoices;

public class GetNumberOfInvoicesDto
{
	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public int Count { get; set; }

	public Guid? CustomerId { get; set; }
}
