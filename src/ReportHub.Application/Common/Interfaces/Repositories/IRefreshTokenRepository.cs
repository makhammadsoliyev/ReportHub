using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
	Task InsertAsync(RefreshToken refreshToken);

	Task UpdateAsync(RefreshToken refreshToken);

	Task<RefreshToken> SelectAsync(Expression<Func<RefreshToken, bool>> expression);
}
