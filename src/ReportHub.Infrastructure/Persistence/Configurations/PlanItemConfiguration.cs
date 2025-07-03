using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Configurations;

public class PlanItemConfiguration : IEntityTypeConfiguration<PlanItem>
{
	public void Configure(EntityTypeBuilder<PlanItem> builder)
	{
		builder.HasOne(planItem => planItem.Plan)
			.WithMany(plan => plan.PlanItems)
			.HasForeignKey(planItem => planItem.PlanId)
			.IsRequired();

		builder.HasOne(planItem => planItem.Organization)
			.WithMany()
			.HasForeignKey(planItem => planItem.OrganizationId)
			.IsRequired();

		builder.HasOne(planItem => planItem.Item)
			.WithMany()
			.HasForeignKey(planItem => planItem.ItemId)
			.IsRequired();
	}
}
