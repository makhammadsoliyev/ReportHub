using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using ReportHub.Application.Common.Interfaces;
using ReportHub.Domain;
using ReportHub.Infrastructure.Identity;
using ReportHub.Infrastructure.Persistence;
using ReportHub.Infrastructure.Persistence.Interceptors;
using ReportHub.Infrastructure.Time;
using ReportHub.Infrastructure.Token;
using ReportHub.Infrastructure.Users;

namespace ReportHub.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
	{
		services.AddServices();
		services.AddPersistence();

		return services;
	}

	private static void AddPersistence(this IServiceCollection services)
	{
		services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
			options
				.UseNpgsql("Server=dpg-d1defjvfte5s73fipthg-a.oregon-postgres.render.com;Port=5432;Database=reporthub;Userid=reporthub_user;Password=6uvQfKOSLAuH1oZjH4uGEhWcMYHZKN0J;")
				.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>())
				.UseSnakeCaseNamingConvention());

		services.AddScoped<ApplicationDbContextInitializer>();

		services.AddScoped<ISaveChangesInterceptor, SoftDeleteInterceptor>();
		services.AddScoped<ISaveChangesInterceptor, AuditableInterceptor>();
	}

	private static void AddServices(this IServiceCollection services)
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
		services.AddScoped<IIdentityService, IdentityService>();
		services.AddOptions<JwtOptions>().BindConfiguration(nameof(JwtOptions));

		services.AddScoped<ITokenService, TokenService>();
		services.AddSingleton<IDateTimeService, DateTimeService>();

		services.AddHttpContextAccessor();
		services.AddScoped<ICurrentUserService, CurrentUserService>();
	}
}
