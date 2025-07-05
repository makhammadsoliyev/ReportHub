using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface ICustomerRepository
{
	Task InsertAsync(Customer customer);

	Task InsertAsync(IEnumerable<Customer> customers);

	Task UpdateAsync(Customer customer);

	Task<bool> DeleteAsync(Customer customer);

	Task<Customer> SelectAsync(Expression<Func<Customer, bool>> expression);

	IQueryable<Customer> SelectAll();

	Task<bool> AnyAsync(Expression<Func<Customer, bool>> expression);
}
