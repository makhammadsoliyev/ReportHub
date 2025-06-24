using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReportHub.Application.Common.Interfaces;
using ReportHub.Domain;
using ReportHub.Infrastructure.Identity;
using ReportHub.Infrastructure.Persistence.Contexts;
using ReportHub.Infrastructure.Time;
using ReportHub.Infrastructure.Token;

namespace ReportHub.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
	{
		services.AddScoped<ITokenService, TokenService>();
		services.AddScoped<IIdentityService, IdentityService>();
		services.AddSingleton<IDateTimeService, DateTimeService>();
		services.AddServices();
		services.AddPersistence();

		return services;
	}

	private static void AddPersistence(this IServiceCollection services)
	{
		services.AddDbContext<ApplicationDbContext>(options =>
			options
				.UseNpgsql("Server=dpg-d1defjvfte5s73fipthg-a.oregon-postgres.render.com;Port=5432;Database=reporthub;Userid=reporthub_user;Password=6uvQfKOSLAuH1oZjH4uGEhWcMYHZKN0J;")
				.UseSnakeCaseNamingConvention());
	}

	public static void AddServices(this IServiceCollection services)
	{
		services.AddIdentityCore<User>(options =>
		{
			options.User.RequireUniqueEmail = true;
			options.Password.RequiredLength = 8;
			options.Password.RequireDigit = true;
			options.Password.RequireUppercase = true;
			options.Password.RequireLowercase = true;
			options.Password.RequireNonAlphanumeric = false;
		})
		.AddRoles<Role>()
		.AddEntityFrameworkStores<ApplicationDbContext>()
		.AddDefaultTokenProviders();

		services.AddDataProtection();
	}
}
