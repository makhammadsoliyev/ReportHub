using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
	public async Task<bool> AnyAsync(Expression<Func<User, bool>> expression)
	{
		return await context.Users.AnyAsync(expression);
	}

	public async Task<bool> DeleteAsync(User user)
	{
		context.Users.Remove(user);
		return await context.SaveChangesAsync() > 0;
	}

	public async Task<User> Select(Expression<Func<User, bool>> expression)
	{
		return await context.Users.FirstOrDefaultAsync(expression);
	}

	public IQueryable<User> SelectAll()
	{
		return context.Users;
	}
}
