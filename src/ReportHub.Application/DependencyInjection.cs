using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ReportHub.Application.Common.Behaviors;

namespace ReportHub.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
	{
		var assembly = Assembly.GetExecutingAssembly();

		services.AddAutoMapper(assembly);
		services.AddValidatorsFromAssembly(assembly);
		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssembly(assembly);
			config.AddOpenBehavior(typeof(LoggingBehavior<,>));
		});

		return services;
	}
}
