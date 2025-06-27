using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface IOrganizationRepository
{
	public Task InsertAsync(Organization organization);

	public Task<bool> AnyAsync(Expression<Func<Organization, bool>> expression);
}
