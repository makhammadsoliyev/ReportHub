using MongoDB.Driver;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.MongoDb;

public class MongoDbContext(IMongoDatabase database)
{
	public IMongoCollection<Log> Logs => database.GetCollection<Log>(nameof(Logs));
}
