using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface IItemRepository
{
	Task InsertAsync(Item item);

	Task UpdateAsync(Item item);

	Task<bool> DeleteAsync(Item item);

	Task<Item> SelectAsync(Expression<Func<Item, bool>> expression);

	IQueryable<Item> SelectAll();

	Task<bool> AnyAsync(Expression<Func<Item, bool>> expression);
}
