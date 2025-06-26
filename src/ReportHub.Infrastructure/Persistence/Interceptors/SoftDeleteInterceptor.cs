using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ReportHub.Application.Common.Interfaces;
using ReportHub.Domain.Common;

namespace ReportHub.Infrastructure.Persistence.Interceptors;

public class SoftDeleteInterceptor(IDateTimeService dateTimeService, ICurrentUserService currentUserService) : SaveChangesInterceptor
{
	public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
	{
		if (eventData.Context is null)
		{
			return base.SavedChangesAsync(eventData, result, cancellationToken);
		}

		var entries = eventData.Context.ChangeTracker
			.Entries<ISoftDeletable>()
			.Where(entry => entry.State == EntityState.Deleted);

		foreach (var entry in entries)
		{
			entry.Entity.IsDeleted = true;
			entry.State = EntityState.Modified;
			entry.Entity.DeletedBy = currentUserService.UserId;
			entry.Entity.DeletedOnUtc = dateTimeService.UtcNow;
		}

		return base.SavedChangesAsync(eventData, result, cancellationToken);
	}
}
