using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentOrganizationService service)
	: IdentityDbContext<User, Role, Guid>(options), IDataProtectionKeyContext
{
	public DbSet<Customer> Customers { get; set; }

	public DbSet<Invoice> Invoices { get; set; }

	public DbSet<InvoiceItem> InvoiceItems { get; set; }

	public DbSet<Item> Items { get; set; }

	public DbSet<Organization> Organizations { get; set; }

	public DbSet<OrganizationMember> OrganizationMembers { get; set; }

	public DbSet<OrganizationRole> OrganizationRoles { get; set; }

	public DbSet<Plan> Plans { get; set; }

	public DbSet<PlanItem> PlanItems { get; set; }

	public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

		base.OnModelCreating(builder);

		ApplyIdentityEntityConfigurations(builder);
		ApplyEntityQueryFilters(builder);
	}

	private static void ApplyIdentityEntityConfigurations(ModelBuilder builder)
	{
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

	private void ApplyEntityQueryFilters(ModelBuilder builder)
	{
		builder.Entity<Customer>().HasQueryFilter(t => t.OrganizationId == service.OrganizationId && !t.IsDeleted);
		builder.Entity<Invoice>().HasQueryFilter(t => t.OrganizationId == service.OrganizationId && !t.IsDeleted);
		builder.Entity<InvoiceItem>().HasQueryFilter(t => t.OrganizationId == service.OrganizationId && !t.IsDeleted);
		builder.Entity<Item>().HasQueryFilter(t => t.OrganizationId == service.OrganizationId && !t.IsDeleted);
		builder.Entity<OrganizationMember>().HasQueryFilter(t => t.OrganizationId == service.OrganizationId && !t.IsDeleted);
		builder.Entity<Plan>().HasQueryFilter(t => t.OrganizationId == service.OrganizationId && !t.IsDeleted);
		builder.Entity<PlanItem>().HasQueryFilter(t => t.OrganizationId == service.OrganizationId && !t.IsDeleted);
		builder.Entity<Organization>().HasQueryFilter(t => !t.IsDeleted);
		builder.Entity<OrganizationRole>().HasQueryFilter(t => !t.IsDeleted);
		builder.Entity<Role>().HasQueryFilter(t => !t.IsDeleted);
		builder.Entity<User>().HasQueryFilter(t => !t.IsDeleted);
	}
}
