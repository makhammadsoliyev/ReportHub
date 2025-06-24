using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Contexts;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<User, Role, Guid>(options)
{
	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

		base.OnModelCreating(builder);

		builder.Ignore<IdentityUserLogin<Guid>>();
		builder.Ignore<IdentityUserToken<Guid>>();
		builder.Ignore<IdentityUserClaim<Guid>>();
		builder.Ignore<IdentityRoleClaim<Guid>>();

		builder.Entity<User>()
			.Ignore(user => user.LockoutEnd)
			.Ignore(user => user.PhoneNumber)
			.Ignore(user => user.LockoutEnabled)
			.Ignore(user => user.TwoFactorEnabled)
			.Ignore(user => user.AccessFailedCount)
			.Ignore(user => user.PhoneNumberConfirmed)
			.ToTable("users");

		builder.Entity<Role>().ToTable("roles");
		builder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");
	}
}
