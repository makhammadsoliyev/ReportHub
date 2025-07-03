using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface IPlanRepository
{
	Task InsertAsync(Plan plan);

	Task UpdateAsync(Plan plan);

	Task<bool> DeleteAsync(Plan plan);

	Task<Plan> SelectAsync(Expression<Func<Plan, bool>> expression);

	IQueryable<Plan> SelectAll();

	Task<bool> AnyAsync(Expression<Func<Plan, bool>> expression);
}
