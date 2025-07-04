using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ReportHub.Domain;
using ReportHub.Infrastructure.Persistence.MongoDb;

namespace ReportHub.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController(MongoDbContext context) : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> PostAsync([FromBody] Log log)
		{
			await context.Logs.InsertOneAsync(log);
			return Ok();
		}

		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			return Ok(await context.Logs.AsQueryable().ToListAsync());
		}
	}
}
