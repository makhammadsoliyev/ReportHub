using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Repositories;

public class PlanRepository(ApplicationDbContext context) : IPlanRepository
{
	public async Task<bool> AnyAsync(Expression<Func<Plan, bool>> expression)
	{
		return await context.Plans.AnyAsync(expression);
	}

	public async Task<bool> DeleteAsync(Plan plan)
	{
		context.Plans.Remove(plan);
		return await context.SaveChangesAsync() > 0;
	}

	public async Task InsertAsync(Plan plan)
	{
		await context.AddAsync(plan);
		await context.SaveChangesAsync();
	}

	public IQueryable<Plan> SelectAll()
	{
		return context.Plans.Include(t => t.PlanItems);
	}

	public async Task<Plan> SelectAsync(Expression<Func<Plan, bool>> expression)
	{
		return await context.Plans.Include(t => t.PlanItems).FirstOrDefaultAsync(expression);
	}

	public async Task UpdateAsync(Plan plan)
	{
		context.Update(plan);
		await context.SaveChangesAsync();
	}
}
