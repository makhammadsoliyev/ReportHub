using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Repositories;

public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
	public async Task<bool> DeleteAsync(Customer customer)
	{
		context.Remove(customer);
		return await context.SaveChangesAsync() > 0;
	}

	public async Task InsertAsync(Customer customer)
	{
		await context.AddAsync(customer);
		await context.SaveChangesAsync();
	}

	public IQueryable<Customer> SelectAll()
	{
		return context.Customers;
	}

	public async Task<Customer> SelectAsync(Expression<Func<Customer, bool>> expression)
	{
		return await context.Customers.FirstOrDefaultAsync(expression);
	}

	public async Task UpdateAsync(Customer customer)
	{
		context.Update(customer);
		await context.SaveChangesAsync();
	}
}
