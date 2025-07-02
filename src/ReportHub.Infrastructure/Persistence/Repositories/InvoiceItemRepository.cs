using System.Linq.Expressions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Repositories;

public class InvoiceItemRepository(ApplicationDbContext context) : IInvoiceItemRepository
{
	public async Task<bool> DeleteAsync(InvoiceItem invoiceItem)
	{
		context.Remove(invoiceItem);
		return await context.SaveChangesAsync() > 0;
	}

	public async Task InsertAsync(InvoiceItem invoiceItem)
	{
		await context.AddAsync(invoiceItem);
		await context.SaveChangesAsync();
	}

	public IQueryable<InvoiceItem> SelectAll()
	{
		return context.InvoiceItems;
	}

	public Task<InvoiceItem> SelectAsync(Expression<Func<InvoiceItem, bool>> expression)
	{
		throw new NotImplementedException();
	}
}
