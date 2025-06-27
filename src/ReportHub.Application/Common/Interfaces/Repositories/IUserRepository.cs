using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
	Task<bool> AnyAsync(Expression<Func<User, bool>> expression);

	IQueryable<User> SelectAll();

	Task<User> Select(Expression<Func<User, bool>> expression);
}
