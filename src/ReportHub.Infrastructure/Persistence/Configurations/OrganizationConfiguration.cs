using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Configurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
	public void Configure(EntityTypeBuilder<Organization> builder)
	{
		builder.Property(organization => organization.Name)
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(organization => organization.CountryCode)
			.HasMaxLength(5)
			.IsRequired();
	}
}
