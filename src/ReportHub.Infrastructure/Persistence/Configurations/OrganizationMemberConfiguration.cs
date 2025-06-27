using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Configurations;

public class OrganizationMemberConfiguration : IEntityTypeConfiguration<OrganizationMember>
{
	public void Configure(EntityTypeBuilder<OrganizationMember> builder)
	{
		builder.HasOne(organizationMember => organizationMember.User)
			.WithMany()
			.HasForeignKey(organizationMember => organizationMember.UserId)
			.IsRequired();

		builder.HasOne(organizationMember => organizationMember.Organization)
			.WithMany()
			.HasForeignKey(organizationMember => organizationMember.OrganizationId)
			.IsRequired();

		builder.HasOne(organizationMember => organizationMember.OrganizationRole)
			.WithMany()
			.HasForeignKey(organizationMember => organizationMember.OrganizationRoleId)
			.IsRequired();
	}
}
