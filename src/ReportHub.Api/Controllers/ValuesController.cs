using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Domain;
using ReportHub.Infrastructure.Persistence.MongoDb;

namespace ReportHub.Api.Controllers
{
	[Route("api/values")]
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
	}
}
