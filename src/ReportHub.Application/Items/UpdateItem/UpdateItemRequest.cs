namespace ReportHub.Application.Items.UpdateItem;

public class UpdateItemRequest
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public decimal Price { get; set; }

	public string CurrencyCode { get; set; }
}
