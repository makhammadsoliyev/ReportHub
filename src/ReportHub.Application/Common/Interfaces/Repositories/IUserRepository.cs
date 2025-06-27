using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
	public Task<bool> AnyAsync(Expression<Func<User, bool>> expression);
}
