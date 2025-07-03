using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Repositories;

public class PlanItemRepository(ApplicationDbContext context) : IPlanItemRepository
{
	public async Task<bool> DeleteAsync(PlanItem planItem)
	{
		context.Remove(planItem);
		return await context.SaveChangesAsync() > 0;
	}

	public async Task InsertAsync(PlanItem planItem)
	{
		await context.AddAsync(planItem);
		await context.SaveChangesAsync();
	}

	public IQueryable<PlanItem> SelectAll()
	{
		return context.PlanItems;
	}

	public async Task<PlanItem> SelectAsync(Expression<Func<PlanItem, bool>> expression)
	{
		return await context.PlanItems.FirstOrDefaultAsync(expression);
	}
}
