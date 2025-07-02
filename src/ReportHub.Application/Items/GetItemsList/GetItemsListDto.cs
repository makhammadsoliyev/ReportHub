﻿namespace ReportHub.Application.Items.GetItemsList;

public class GetItemsListDto
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public decimal Price { get; set; }

	public string CurrencyCode { get; set; }
}
