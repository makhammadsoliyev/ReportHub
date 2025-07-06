using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
	public void Configure(EntityTypeBuilder<RefreshToken> builder)
	{
		builder.Property(refreshToken => refreshToken.Token)
			.HasMaxLength(200)
			.IsRequired();

		builder.HasIndex(refreshToken => refreshToken.Token)
			.IsUnique();

		builder.HasOne(refreshToken => refreshToken.User)
			.WithMany()
			.HasForeignKey(refreshToken => refreshToken.UserId)
			.IsRequired();
	}
}
