using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Repositories;

public class OrganizationMemberRepository(ApplicationDbContext context) : IOrganizationMemberRepository
{
	public async Task<bool> AnyAsync(Expression<Func<OrganizationMember, bool>> expression)
	{
		return await context.OrganizationMembers.AnyAsync(expression);
	}

	public async Task InsertAsync(OrganizationMember organizationMember)
	{
		await context.AddAsync(organizationMember);
		await context.SaveChangesAsync();
	}

	public IQueryable<OrganizationMember> SelectAll()
	{
		return context.OrganizationMembers
			.Include(t => t.User)
			.Include(t => t.OrganizationRole)
			.Include(t => t.Organization);
	}

	public async Task<List<string>> SelectRoleAsync(Guid userId)
	{
		return await context.OrganizationMembers
			.Where(t => t.UserId == userId)
			.Include(t => t.OrganizationRole)
			.Select(t => t.OrganizationRole.Name)
			.ToListAsync();
	}
}
