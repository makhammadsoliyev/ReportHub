using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface ILogRepository
{
	Task InsertAsync(Log log);

	Task<Log> SelectAsync(Expression<Func<Log, bool>> expression);

	Task<List<Log>> SelectAllAsync();
}
