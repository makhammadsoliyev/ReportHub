using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<User, Role, Guid>(options)
{
	public DbSet<Customer> Customers { get; set; }

	public DbSet<Invoice> Invoices { get; set; }

	public DbSet<InvoiceItem> InvoiceItems { get; set; }

	public DbSet<Item> Items { get; set; }

	public DbSet<Organization> Organizations { get; set; }

	public DbSet<OrganizationMember> OrganizationMembers { get; set; }

	public DbSet<OrganizationRole> OrganizationRoles { get; set; }

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
