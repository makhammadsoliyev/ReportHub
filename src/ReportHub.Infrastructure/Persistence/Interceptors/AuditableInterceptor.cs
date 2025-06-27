using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Domain.Common;

namespace ReportHub.Infrastructure.Persistence.Interceptors;

public class AuditableInterceptor(IDateTimeService dateTimeService, ICurrentUserService currentUserService) : SaveChangesInterceptor
{
	public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
	{
		if (eventData.Context is null)
		{
			return base.SavedChangesAsync(eventData, result, cancellationToken);
		}

		var entries = eventData.Context.ChangeTracker.Entries<IAuditable>();

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

		return base.SavedChangesAsync(eventData, result, cancellationToken);
	}
}
