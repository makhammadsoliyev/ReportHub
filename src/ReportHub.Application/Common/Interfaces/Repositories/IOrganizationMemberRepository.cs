using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface IOrganizationMemberRepository
{
	Task InsertAsync(OrganizationMember organizationMember);

	Task<bool> AnyAsync(Expression<Func<OrganizationMember, bool>> expression);
}
