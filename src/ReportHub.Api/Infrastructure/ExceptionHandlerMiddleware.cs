using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.Common.Exceptions;

namespace ReportHub.Api.Infrastructure;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (Exception exception)
		{
			var detail = exception.Message;
			var instance = context.Request.Path;
			var title = exception.GetType().Name;

			var problemDetails = exception switch
			{
				NotFoundException => new ProblemDetails
				{
					Title = title,
					Detail = detail,
					Instance = instance,
					Status = StatusCodes.Status404NotFound,
					Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
				},
				BadRequestException => new ProblemDetails
				{
					Title = title,
					Detail = detail,
					Instance = instance,
					Status = StatusCodes.Status400BadRequest,
					Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.2",
				},
				ConflictException => new ProblemDetails
				{
					Title = title,
					Detail = detail,
					Instance = instance,
					Status = StatusCodes.Status409Conflict,
					Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
				},
				ForbiddenException => new ProblemDetails
				{
					Title = title,
					Detail = detail,
					Instance = instance,
					Status = StatusCodes.Status403Forbidden,
					Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
				},
				UnauthorizedException => new ProblemDetails
				{
					Title = title,
					Detail = detail,
					Instance = instance,
					Status = StatusCodes.Status401Unauthorized,
					Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
				},
				ValidationException validationException => new ProblemDetails
				{
					Title = title,
					Detail = detail,
					Instance = instance,
					Status = StatusCodes.Status400BadRequest,
					Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.2",
					Extensions =
					{
						["errors"] = validationException.Errors,
					},
				},
				_ => new ProblemDetails
				{
					Title = title,
					Detail = detail,
					Instance = instance,
					Status = StatusCodes.Status500InternalServerError,
					Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
				},
			};

			if (problemDetails.Status.Value == StatusCodes.Status500InternalServerError)
			{
				logger.LogError(exception, "Internal Server Error");
			}

			context.Response.StatusCode = problemDetails.Status.Value;

			await context.Response.WriteAsJsonAsync(problemDetails);
		}
	}
}
