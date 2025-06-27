using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Configurations;

public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
	public void Configure(EntityTypeBuilder<InvoiceItem> builder)
	{
		builder.Property(invoiceItem => invoiceItem.CurrencyCode)
			.HasMaxLength(5)
			.IsRequired();

		builder.HasOne(invoiceItem => invoiceItem.Invoice)
			.WithMany()
			.HasForeignKey(invoiceItem => invoiceItem.InvoiceId)
			.IsRequired();

		builder.HasOne(invoiceItem => invoiceItem.Item)
			.WithMany()
			.HasForeignKey(invoiceItem => invoiceItem.ItemId)
			.IsRequired();

		builder.HasOne(invoiceItem => invoiceItem.Organization)
			.WithMany()
			.HasForeignKey(invoiceItem => invoiceItem.OrganizationId)
			.IsRequired();
	}
}
