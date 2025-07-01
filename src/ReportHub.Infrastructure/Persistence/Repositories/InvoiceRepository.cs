using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Repositories;

public class InvoiceRepository(ApplicationDbContext context) : IInvoiceRepository
{
	public async Task InsertAsync(Invoice invoice)
	{
		await context.AddAsync(invoice);
		await context.SaveChangesAsync();
	}
}
