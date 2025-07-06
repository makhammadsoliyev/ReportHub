using ReportHub.Domain.Common;

namespace ReportHub.Domain;

public class RefreshToken : SoftDeletableAuditableEntity
{
	public string Token { get; set; }

	public Guid UserId { get; set; }

	public User User { get; set; }

	public DateTime ExpiresOnUtc { get; set; }
}
