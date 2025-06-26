using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReportHub.Application.Common.Constants;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence;

public class ApplicationDbContextInitializer(
	ILogger<ApplicationDbContextInitializer> logger,
	ApplicationDbContext context,
	UserManager<User> userManager,
	RoleManager<Role> roleManager)
{
	public async Task InitialiseAsync()
	{
		try
		{
			await context.Database.MigrateAsync();
		}
		catch (Exception exception)
		{
			logger.LogError(exception, "An error occurred while initialising the database.");
			throw;
		}
	}

	public async Task SeedAsync()
	{
		try
		{
			await TrySeedAsync();
		}
		catch (Exception exception)
		{
			logger.LogError(exception, "An error occurred while seeding the database.");
			throw;
		}
	}

	private async Task TrySeedAsync()
	{
		if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
		{
			await roleManager.CreateAsync(new Role { Name = UserRoles.Admin });
		}

		if (!await roleManager.RoleExistsAsync(UserRoles.User))
		{
			await roleManager.CreateAsync(new Role { Name = UserRoles.User });
		}

		var admin = new User
		{
			UserName = "admin@exadel.com",
			Email = "admin@exadel.com",
			EmailConfirmed = true,
		};

		if (!await userManager.Users.AnyAsync(user => user.UserName == admin.UserName))
		{
			await userManager.CreateAsync(admin, "Admin1234");
			await userManager.AddToRoleAsync(admin, UserRoles.Admin);
		}
	}
}
