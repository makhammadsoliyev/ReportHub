namespace ReportHub.Application.Items.GetItemById;

public class GetItemByIdDto
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public decimal Price { get; set; }

	public string CurrencyCode { get; set; }
}
