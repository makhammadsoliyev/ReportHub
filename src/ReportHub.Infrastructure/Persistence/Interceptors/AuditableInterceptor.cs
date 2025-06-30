using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Domain.Common;

namespace ReportHub.Infrastructure.Persistence.Interceptors;

public class AuditableInterceptor(IDateTimeService dateTimeService, ICurrentUserService currentUserService) : SaveChangesInterceptor
{
	public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
	{
		UpdateAuditProperties(eventData.Context);

		return base.SavedChangesAsync(eventData, result, cancellationToken);
	}

	public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
	{
		UpdateAuditProperties(eventData.Context);

		return base.SavedChanges(eventData, result);
	}

	private void UpdateAuditProperties(DbContext context)
	{
		if (context == null)
		{
			return;
		}

		var entries = context.ChangeTracker.Entries<IAuditable>();

		foreach (var entry in entries)
		{
			if (entry.State == EntityState.Added)
			{
				entry.Entity.CreatedOnUtc = dateTimeService.UtcNow;
				entry.Entity.CreatedBy = currentUserService.UserId;
			}
			else if (entry.State == EntityState.Modified)
			{
				entry.Entity.UpdatedOnUtc = dateTimeService.UtcNow;
				entry.Entity.UpdatedBy = currentUserService.UserId;
			}
		}
	}
}
