using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Services;

public interface IAsposeService
{
	Task<byte[]> GenerateAsync(IQueryable<Invoice> invoices, IQueryable<Item> items, IQueryable<Plan> plans);

	IEnumerable<Customer> ImportCustomers(Stream fileStream);
}
