using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface IOrganizationRoleRepository
{
	Task<OrganizationRole> SelectAsync(Expression<Func<OrganizationRole, bool>> expression);

	IQueryable<OrganizationRole> SelectAll();
}
