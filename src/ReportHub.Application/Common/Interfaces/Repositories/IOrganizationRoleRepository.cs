using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface IOrganizationRoleRepository
{
    public Task<OrganizationRole> SelectAsync(Expression<Func<OrganizationRole, bool>> expression);
}
