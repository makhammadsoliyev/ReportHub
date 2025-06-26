using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.Users.LoginUser;
using ReportHub.Application.Users.RegisterUser;

namespace ReportHub.Api.Controllers;

public class UsersController(ISender mediator) : BaseController(mediator)
{
	[HttpPost("register")]
	public async Task<IActionResult> LoginAsync([FromBody] RegisterUserCommand command)
	{
		var result = await Mediator.Send(command);

		throw new Exception("{\r\n  \"firstName\": \"string\",\r\n  \"lastName\": \"string\",\r\n  \"email\": \"string2asfgcdkj@gm.com\",\r\n  \"password\": \"S126512tring\"\r\n}");

		return Ok(result);
	}

	[HttpPost("login")]
	public async Task<IActionResult> LoginAsync([FromBody] LoginUserCommand command)
	{
		var result = await Mediator.Send(command);

		return Ok(result);
	}
}
