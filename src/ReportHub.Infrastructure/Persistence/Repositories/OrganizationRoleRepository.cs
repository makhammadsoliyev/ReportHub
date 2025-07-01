using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Repositories;

public class OrganizationRoleRepository(ApplicationDbContext context) : IOrganizationRoleRepository
{
	public async Task<bool> AnyAsync(Expression<Func<OrganizationRole, bool>> expression)
	{
		return await context.OrganizationRoles.AnyAsync(expression);
	}

	public IQueryable<OrganizationRole> SelectAll()
	{
		return context.OrganizationRoles;
	}

	public async Task<OrganizationRole> SelectAsync(Expression<Func<OrganizationRole, bool>> expression)
	{
		return await context.OrganizationRoles.FirstOrDefaultAsync(expression);
	}
}
