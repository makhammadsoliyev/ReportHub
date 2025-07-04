using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Domain;
using ReportHub.Infrastructure.Persistence.MongoDb;

namespace ReportHub.Infrastructure.Persistence.Repositories;

public class LogRepository(MongoDbContext context) : ILogRepository
{
	public async Task InsertAsync(Log log)
	{
		await context.Logs.InsertOneAsync(log);
	}

	public async Task<List<Log>> SelectAllAsync()
	{
		return await context.Logs.AsQueryable().ToListAsync();
	}

	public async Task<Log> SelectAsync(Expression<Func<Log, bool>> expression)
	{
		return await context.Logs.Find(expression).FirstOrDefaultAsync();
	}
}
