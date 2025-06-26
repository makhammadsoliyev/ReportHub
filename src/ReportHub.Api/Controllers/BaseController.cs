using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ReportHub.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BaseController : ControllerBase
{
	protected BaseController(ISender mediator)
	{
		Mediator = mediator;
	}

	protected ISender Mediator { get; }
}
