using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface IOrganizationRepository
{
	Task InsertAsync(Organization organization);

	Task<bool> AnyAsync(Expression<Func<Organization, bool>> expression);

	Task<bool> DeleteAsync(Organization organization);

	Task UpdateAsync(Organization organization);

	Task<Organization> SelectAsync(Expression<Func<Organization, bool>> expression);

	IQueryable<Organization> SelectAll();
}
