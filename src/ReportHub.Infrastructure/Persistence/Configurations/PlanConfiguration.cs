using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Configurations;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
	public void Configure(EntityTypeBuilder<Plan> builder)
	{
		builder.Property(plan => plan.Title)
			.HasMaxLength(200)
			.IsRequired();

		builder.Ignore(plan => plan.Price);
		builder.Ignore(plan => plan.ItemsCount);
	}
}
