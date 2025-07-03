using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface IPlanItemRepository
{
	Task InsertAsync(PlanItem planItem);

	Task<bool> DeleteAsync(PlanItem planItem);

	Task<PlanItem> SelectAsync(Expression<Func<PlanItem, bool>> expression);

	IQueryable<PlanItem> SelectAll();
}
