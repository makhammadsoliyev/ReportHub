using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.Property(user => user.Email)
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(user => user.FirstName)
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(user => user.LastName)
			.HasMaxLength(200)
			.IsRequired();

		builder.HasIndex(user => user.Email)
			.IsUnique();
	}
}
