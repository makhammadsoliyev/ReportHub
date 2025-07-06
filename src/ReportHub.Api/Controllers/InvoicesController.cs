using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.Invoices.AddInvoiceItem;
using ReportHub.Application.Invoices.CreateInvoice;
using ReportHub.Application.Invoices.DeleteInvoice;
using ReportHub.Application.Invoices.DeleteInvoiceItem;
using ReportHub.Application.Invoices.ExportInvoice;
using ReportHub.Application.Invoices.GetInvoiceById;
using ReportHub.Application.Invoices.GetInvoiceItemList;
using ReportHub.Application.Invoices.GetInvoicesList;
using ReportHub.Application.Invoices.GetInvoicesRevenueCalculation;
using ReportHub.Application.Invoices.GetLogById;
using ReportHub.Application.Invoices.GetLogsList;
using ReportHub.Application.Invoices.GetNumberOfInvoices;
using ReportHub.Application.Invoices.GetOverdueInvoicesAnalysis;
using ReportHub.Application.Invoices.UpdateInvoice;

namespace ReportHub.Api.Controllers;

[Route("organizations/{organizationId:guid}/invoices")]
public class InvoicesController(ISender mediator) : BaseController(mediator)
{
	[HttpPost]
	public async Task<IActionResult> CreateAsync([FromBody] CreateInvoiceRequest request, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new CreateInvoiceCommand(request, organizationId));

		return Ok(result);
	}

	[HttpPut]
	public async Task<IActionResult> UpdateAsync([FromBody] UpdateInvoiceRequest request, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new UpdateInvoiceCommand(request, organizationId));

		return Ok(result);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new DeleteInvoiceCommand(id, organizationId));

		return Ok(result);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, [FromRoute,] Guid organizationId)
	{
		var result = await Mediator.Send(new GetInvoiceByIdQuery(id, organizationId));

		return Ok(result);
	}

	[HttpGet]
	public async Task<IActionResult> GetListAsync([FromRoute,] Guid organizationId)
	{
		var result = await Mediator.Send(new GetInvoicesListQuery(organizationId));

		return Ok(result);
	}

	[HttpPost("{id:guid}/items")]
	public async Task<IActionResult> AddItemAsync(
		[FromBody] AddInvoiceItemRequest request, [FromRoute] Guid id, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new AddInvoiceItemCommand(request, id, organizationId));

		return Ok(result);
	}

	[HttpDelete("{id:guid}/items/{invoiceItemId:guid}")]
	public async Task<IActionResult> DeleteItemAsync([FromRoute] Guid id, [FromRoute] Guid invoiceItemId, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new DeleteInvoiceItemCommand(invoiceItemId, id, organizationId));

		return Ok(result);
	}

	[HttpGet("{id:guid}/items")]
	public async Task<IActionResult> GetItemsListAsync([FromRoute] Guid id, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new GetInvoiceItemListQuery(id, organizationId));

		return Ok(result);
	}

	[HttpGet("{id:guid}/export-pdf")]
	public async Task<IActionResult> ExportAsync([FromRoute] Guid id, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new ExportInvoiceQuery(id, organizationId));

		return File(result.Content, result.ContentType, result.FileName);
	}

	[HttpGet("count")]
	public async Task<IActionResult> GetCountAsync([FromQuery] GetNumberOfInvoicesFilter filter, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new GetNumberOfInvoicesQuery(filter, organizationId));

		return Ok(result);
	}

	[HttpGet("revenue")]
	public async Task<IActionResult> GetRevenueAsync([FromQuery] GetInvoicesRevenueCalculationFilter filter, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new GetInvoicesRevenueCalculationQuery(filter, organizationId));

		return Ok(result);
	}

	[HttpGet("overdue-analysis")]
	public async Task<IActionResult> GetOverdueAnalysisAsync([FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new GetOverdueInvoicesAnalysisQuery(organizationId));

		return Ok(result);
	}

	[HttpGet("logs/{id:guid}")]
	public async Task<IActionResult> GetLogAsync([FromRoute] Guid id, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new GetLogByIdQuery(id, organizationId));

		return Ok(result);
	}

	[HttpGet("logs")]
	public async Task<IActionResult> GetLogsListAsync([FromQuery] GetLogsListFilter filter, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new GetLogsListQuery(filter, organizationId));

		return Ok(result);
	}
}
