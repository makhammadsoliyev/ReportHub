using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface ICustomerRepository
{
	Task InsertAsync(Customer customer);
}
