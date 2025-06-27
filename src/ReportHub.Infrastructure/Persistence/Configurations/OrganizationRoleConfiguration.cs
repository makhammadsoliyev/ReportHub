using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Configurations;

public class OrganizationRoleConfiguration : IEntityTypeConfiguration<OrganizationRole>
{
	public void Configure(EntityTypeBuilder<OrganizationRole> builder)
	{
		builder.Property(organizationRole => organizationRole.Name)
			.HasMaxLength(200)
			.IsRequired();
	}
}
