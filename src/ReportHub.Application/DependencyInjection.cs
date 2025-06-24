﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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
		});

		return services;
	}
}
