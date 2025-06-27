using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Repositories;

public class OrganizationRepository(ApplicationDbContext context) : IOrganizationRepository
{
	public async Task<bool> AnyAsync(Expression<Func<Organization, bool>> expression)
	{
		return await context.Organizations.AnyAsync(expression);
	}

	public async Task InsertAsync(Organization organization)
	{
		await context.AddAsync(organization);
		await context.SaveChangesAsync();
	}
}
