using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface IOrganizationMemberRepository
{
	public Task InsertAsync(OrganizationMember organizationMember);

	public Task<bool> AnyAsync(Expression<Func<OrganizationMember, bool>> expression);
}
