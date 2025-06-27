using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
	public void Configure(EntityTypeBuilder<Customer> builder)
	{
		builder.Property(customer => customer.Name)
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(customer => customer.Email)
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(customer => customer.CountryCode)
			.HasMaxLength(5)
			.IsRequired();

		builder.HasOne(customer => customer.Organization)
			.WithMany()
			.HasForeignKey(customer => customer.OrganizationId)
			.IsRequired();
	}
}
