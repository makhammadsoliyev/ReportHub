using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
	public void Configure(EntityTypeBuilder<Invoice> builder)
	{
		builder.Property(invoice => invoice.InvoiceNumber)
			.ValueGeneratedOnAdd()
			.IsRequired();

		builder.Property(invoice => invoice.CurrencyCode)
			.HasMaxLength(5)
			.IsRequired();

		builder.HasOne(invoice => invoice.Organization)
			.WithMany()
			.HasForeignKey(invoice => invoice.OrganizationId)
			.IsRequired();

		builder.HasIndex(invoice => invoice.InvoiceNumber);

		builder.Ignore(invoice => invoice.Price);
		builder.Ignore(invoice => invoice.ItemsCount);
	}
}
