using ReportHub.Api.Infrastructure;

namespace ReportHub.Api.Extensions;

public static class ExceptionHandlerExtensions
{
	public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
	{
		app.UseMiddleware<ExceptionHandlerMiddleware>();
	}
}
