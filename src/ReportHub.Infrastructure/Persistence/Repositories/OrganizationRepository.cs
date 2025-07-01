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

	public async Task<bool> DeleteAsync(Organization organization)
	{
		context.Organizations.Remove(organization);
		return await context.SaveChangesAsync() > 0;
	}

	public async Task InsertAsync(Organization organization)
	{
		await context.AddAsync(organization);
		await context.SaveChangesAsync();
	}

	public IQueryable<Organization> SelectAll()
	{
		return context.Organizations;
	}

	public Task<Organization> SelectAsync(Expression<Func<Organization, bool>> expression)
	{
		return context.Organizations.FirstOrDefaultAsync(expression);
	}

	public async Task UpdateAsync(Organization organization)
	{
		context.Update(organization);
		await context.SaveChangesAsync();
	}
}
