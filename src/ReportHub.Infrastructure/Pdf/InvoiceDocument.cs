using Microsoft.AspNetCore.Hosting;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Pdf;

public class InvoiceDocument(Invoice model, IWebHostEnvironment environment) : IDocument
{
	public Image LogoImage { get; } = Image.FromFile(Path.Combine(environment.WebRootPath, "logo.jpg"));

	public Invoice Model { get; } = model;

	public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

	public void Compose(IDocumentContainer container)
	{
		container
			.Page(page =>
			{
				page.Margin(50);

				page.Header().Element(ComposeHeader);
				page.Content().Element(ComposeContent);

				page.Footer().AlignCenter().Text(text =>
				{
					text.CurrentPageNumber();
					text.Span(" / ");
					text.TotalPages();
				});
			});
	}

	private void ComposeHeader(IContainer container)
	{
		container.Row(row =>
		{
			row.RelativeItem().Column(column =>
			{
				column
					.Item().Text($"Invoice #{Model.InvoiceNumber:d6}")
					.FontSize(18).SemiBold().FontColor(Colors.Blue.Medium);

				column.Item().Text(text =>
				{
					text.Span("Issue date: ").SemiBold();
					text.Span($"{Model.IssueDate:d}");
				});

				column.Item().Text(text =>
				{
					text.Span("Due date: ").SemiBold();
					text.Span($"{Model.DueDate:d}");
				});

				column.Item().Text(text =>
				{
					text.Span("Payment status: ").SemiBold();
					text.Span($"{Model.PaymentStatus}");
				});
			});

			row.ConstantItem(120).Image(LogoImage);
		});
	}

	private void ComposeContent(IContainer container)
	{
		container.PaddingVertical(40).Column(column =>
		{
			column.Spacing(20);

			column.Item().Row(row =>
			{
				row.RelativeItem().Component(new OrganizationComponent("Organization", Model.Organization));
				row.ConstantItem(70);
				row.RelativeItem().Component(new CustomerComponent("Customer", Model.Customer));
			});

			column.Item().Element(ComposeTable);

			var totalPrice = Model.Price;
			column.Item().PaddingRight(7).AlignRight().Text($"Total: {totalPrice:F2} {Model.CurrencyCode.ToUpper()}").SemiBold();
		});
	}

	private void ComposeTable(IContainer container)
	{
		var headerStyle = TextStyle.Default.SemiBold();

		container.Table(table =>
		{
			table.ColumnsDefinition(columns =>
			{
				columns.ConstantColumn(27);
				columns.RelativeColumn(5);
				columns.RelativeColumn(3);
				columns.RelativeColumn(2);
				columns.RelativeColumn(2);
				columns.RelativeColumn(3);
				columns.RelativeColumn(2);
			});

			table.Header(header =>
			{
				header.Cell().Element(CellStyle).AlignCenter().Text("#");
				header.Cell().Element(CellStyle).AlignCenter().Text("Item").Style(headerStyle);
				header.Cell().Element(CellStyle).AlignCenter().Text("Price").Style(headerStyle);
				header.Cell().Element(CellStyle).AlignCenter().Text("Currency").Style(headerStyle);
				header.Cell().Element(CellStyle).AlignCenter().Text("Quantity").Style(headerStyle);
				header.Cell().Element(CellStyle).AlignCenter().Text("Total").Style(headerStyle);
				header.Cell().Element(CellStyle).AlignCenter().Text("Currency").Style(headerStyle);

				header.Cell().ColumnSpan(7).BorderBottom(1).BorderColor(Colors.Black);

				static IContainer CellStyle(IContainer container) => container.Border(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(7);
			});

			foreach (var invoiceItem in Model.InvoiceItems)
			{
				var index = Model.InvoiceItems.IndexOf(invoiceItem) + 1;

				table.Cell().Element(CellStyle).AlignCenter().Text($"{index}");
				table.Cell().Element(CellStyle).AlignCenter().Text($"{invoiceItem.Item.Name}");
				table.Cell().Element(CellStyle).AlignCenter().Text($"{invoiceItem.Item.Price:f2}");
				table.Cell().Element(CellStyle).AlignCenter().Text($"{invoiceItem.Item.CurrencyCode.ToUpper()}");
				table.Cell().Element(CellStyle).AlignCenter().Text($"{invoiceItem.ItemsCount}");
				table.Cell().Element(CellStyle).AlignCenter().Text($"{invoiceItem.Price:f2}");
				table.Cell().Element(CellStyle).AlignCenter().Text($"{invoiceItem.CurrencyCode.ToUpper()}");

				static IContainer CellStyle(IContainer container) => container.Border(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(7);
			}
		});
	}
}
