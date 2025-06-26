using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace ReportHub.Api.Extensions;

public static class SwaggerExtensions
{
	public static void AddSwaggerGenWithUi(this IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
			{
				Title = "Report Hub",
				Version = "v1",
			});

			var securityScheme = new OpenApiSecurityScheme
			{
				Name = "Authorization",
				Description = "Enter your JWT token",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.Http,
				Scheme = JwtBearerDefaults.AuthenticationScheme,
				BearerFormat = "JWT",
			};

			options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

			var securityRequirement = new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = JwtBearerDefaults.AuthenticationScheme,
						},
					},
					[]
				},
			};

			options.AddSecurityRequirement(securityRequirement);
		});
	}

	public static void UseSwaggerUi(this WebApplication app)
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}
}
