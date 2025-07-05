using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.Reports.ExportReport;

namespace ReportHub.Api.Controllers;

[Route("organizations/{organizationId:guid}/[controller]")]
public class ReportsController(ISender mediator) : BaseController(mediator)
{
	[HttpGet("export")]
	public async Task<IActionResult> ExportAsync([FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new ExportReportQuery(organizationId));

		return File(result.Content, result.ContentType, result.FileName);
	}
}
