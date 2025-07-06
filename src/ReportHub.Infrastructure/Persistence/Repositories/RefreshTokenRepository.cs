using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(ApplicationDbContext context) : IRefreshTokenRepository
{
	public async Task InsertAsync(RefreshToken refreshToken)
	{
		await context.AddAsync(refreshToken);
		await context.SaveChangesAsync();
	}

	public async Task<RefreshToken> SelectAsync(Expression<Func<RefreshToken, bool>> expression)
	{
		return await context.RefreshTokens.Include(t => t.User).FirstOrDefaultAsync(expression);
	}

	public async Task UpdateAsync(RefreshToken refreshToken)
	{
		context.Update(refreshToken);
		await context.SaveChangesAsync();
	}
}
