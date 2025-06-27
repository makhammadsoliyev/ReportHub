using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Repositories;

public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
	public async Task InsertAsync(Customer customer)
	{
		await context.AddAsync(customer);
		await context.SaveChangesAsync();
	}
}
