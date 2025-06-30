using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface ICustomerRepository
{
	Task InsertAsync(Customer customer);

	Task<Customer> SelectAsync(Expression<Func<Customer, bool>> expression);

	Task<bool> DeleteAsync(Customer customer);

	IQueryable<Customer> SelectAll();
}
