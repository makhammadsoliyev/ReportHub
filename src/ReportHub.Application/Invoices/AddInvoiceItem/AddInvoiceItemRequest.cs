namespace ReportHub.Application.Invoices.AddInvoiceItem;

public class AddInvoiceItemRequest
{
	public Guid ItemId { get; set; }

	public int ItemsCount { get; set; }
}
