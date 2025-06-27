using MediatR;
using Microsoft.Extensions.Logging;
using ReportHub.Application.Common.Interfaces.Services;

namespace ReportHub.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(IDateTimeService dateTimeService, ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var requestName = request.GetType().Name;
		logger.LogInformation("Starting Request: {RequestName} at {DateTime}", requestName, dateTimeService.UtcNow);

		var response = await next(cancellationToken);

		logger.LogInformation("Completed Request: {RequestName} at {DateTime}", requestName, dateTimeService.UtcNow);

		return response;
	}
}