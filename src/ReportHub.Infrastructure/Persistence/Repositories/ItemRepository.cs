using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Repositories;

public class ItemRepository(ApplicationDbContext context) : IItemRepository
{
	public async Task<bool> AnyAsync(Expression<Func<Item, bool>> expression)
	{
		return await context.Items.AnyAsync(expression);
	}

	public async Task<bool> DeleteAsync(Item item)
	{
		context.Remove(item);
		return await context.SaveChangesAsync() > 0;
	}

	public async Task InsertAsync(Item item)
	{
		await context.AddAsync(item);
		await context.SaveChangesAsync();
	}

	public IQueryable<Item> SelectAll()
	{
		return context.Items;
	}

	public async Task<Item> SelectAsync(Expression<Func<Item, bool>> expression)
	{
		return await context.Items.FirstOrDefaultAsync(expression);
	}

	public async Task UpdateAsync(Item item)
	{
		context.Update(item);
		await context.SaveChangesAsync();
	}
}
