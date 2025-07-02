namespace ReportHub.Application.Invoices.GetInvoiceItemList;

public class GetInvoiceItemListDto
{
	public Guid Id { get; set; }

	public Guid ItemId { get; set; }

	public Guid InvoiceId { get; set; }

	public int ItemsCount { get; set; }

	public decimal Price { get; set; }

	public string CurrencyCode { get; set; }
}
