using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ReportHub.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BaseController : ControllerBase
{
	protected readonly ISender mediator;

	protected BaseController(ISender mediator)
	{
		this.mediator = mediator;
	}
}
