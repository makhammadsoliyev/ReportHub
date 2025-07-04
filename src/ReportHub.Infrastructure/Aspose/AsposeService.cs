using System.Collections;
using System.Drawing;
using Aspose.Cells;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Aspose;

public class AsposeService : IAsposeService
{
	public async Task<byte[]> GenerateAsync(IQueryable<Invoice> invoices, IQueryable<Item> items, IQueryable<Plan> plans)
	{
		var workbook = new Workbook();
		workbook.Worksheets.RemoveAt(0);

		var invoiceSummaries = await invoices.Select(invoice => new
		{
			invoice.InvoiceNumber,
			invoice.IssueDate,
			invoice.DueDate,
			invoice.Price,
			invoice.CurrencyCode,
			invoice.ItemsCount,
		})
			.ToListAsync();

		var itemSummaries = await items.Select(item => new
		{
			item.Name,
			item.Price,
			item.CurrencyCode,
		})
			.ToListAsync();

		var planSummaries = await plans.Select(plan => new
		{
			plan.Title,
			plan.Price,
			plan.CurrencyCode,
			plan.StartDate,
			plan.EndDate,
			plan.ItemsCount,
		})
			.ToListAsync();

		ImportValuesToWorksheet(workbook, invoiceSummaries, nameof(invoices));
		ImportValuesToWorksheet(workbook, itemSummaries, nameof(items));
		ImportValuesToWorksheet(workbook, planSummaries, nameof(plans));

		using (var workbookStream = await workbook.SaveToStreamAsync())
		{
			return workbookStream.ToArray();
		}
	}

	private static void ImportValuesToWorksheet(Workbook workbook, ICollection values, string nameOfWorksheet)
	{
		var worksheet = workbook.Worksheets.Add(nameOfWorksheet);
		worksheet.Cells.ImportCustomObjects(values, 0, 1, new ImportTableOptions()
		{
			DateFormat = "MM/dd/yyyy hh:mm",
		});

		worksheet.Cells[0, 0].PutValue("No");
		for (int row = 1; row <= values.Count; row++)
		{
			worksheet.Cells[row, 0].PutValue(row);
		}

		var headerStyle = workbook.CreateStyle();
		headerStyle.Font.IsBold = true;
		headerStyle.ForegroundColor = Color.Azure;
		headerStyle.Pattern = BackgroundType.Solid;

		worksheet.Cells.ApplyRowStyle(0, headerStyle, new StyleFlag { All = true });

		worksheet.AutoFitColumns();
		worksheet.AutoFitRows();
	}
}
