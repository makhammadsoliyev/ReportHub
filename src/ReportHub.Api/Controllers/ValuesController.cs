using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Domain;
using ReportHub.Infrastructure.Persistence.MongoDb;

namespace ReportHub.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController(
		MongoDbContext context,
		ICurrencyService service,
		IAsposeService asposeService,
		IInvoiceRepository repository,
		IItemRepository itemRepository,
		IPlanRepository planRepository) : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> PostAsync([FromBody] Log log)
		{
			await context.Logs.InsertOneAsync(log);
			return Ok();
		}

		[HttpGet]
		public async Task<IActionResult> GetAsync(string from, string to, decimal amount, DateOnly date)
		{
			var result = await service.ExchangeAsync(from, to, amount, date);

			return Ok(result);
		}

		[HttpGet("aspose")]
		public async Task<IActionResult> Aspose()
		{
			var invoices = repository.SelectAll().IgnoreQueryFilters();
			var plans = planRepository.SelectAll().IgnoreQueryFilters();
			var items = itemRepository.SelectAll().IgnoreQueryFilters();

			var result = await asposeService.GenerateAsync(invoices, items, plans);

			return File(result, "application/vnd.ms-excel", "invoice.xls");
		}
	}
}
