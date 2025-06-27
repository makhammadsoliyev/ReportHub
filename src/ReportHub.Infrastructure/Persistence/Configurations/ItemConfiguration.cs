using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
	public void Configure(EntityTypeBuilder<Item> builder)
	{
		builder.Property(item => item.Name)
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(item => item.Description)
			.HasMaxLength(500)
			.IsRequired(false);

		builder.Property(item => item.CurrencyCode)
			.HasMaxLength(5)
			.IsRequired();

		builder.HasOne(customer => customer.Organization)
            .WithMany()
            .HasForeignKey(customer => customer.OrganizationId)
            .IsRequired();
    }
}
