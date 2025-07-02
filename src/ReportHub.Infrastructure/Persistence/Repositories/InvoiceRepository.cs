using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Persistence.Repositories;

public class InvoiceRepository(ApplicationDbContext context) : IInvoiceRepository
{
	public async Task<bool> DeleteAsync(Invoice invoice)
	{
		context.Remove(invoice);

		return await context.SaveChangesAsync() > 0;
	}

	public async Task InsertAsync(Invoice invoice)
	{
		await context.AddAsync(invoice);
		await context.SaveChangesAsync();
	}

	public IQueryable<Invoice> SelectAll()
	{
		return context.Invoices.Include(t => t.InvoiceItems);
	}

	public async Task<Invoice> SelectAsync(Expression<Func<Invoice, bool>> expression)
	{
		return await context.Invoices.Include(t => t.InvoiceItems).FirstOrDefaultAsync(expression);
	}

	public async Task UpdateAsync(Invoice invoice)
	{
		context.Update(invoice);
		await context.SaveChangesAsync();
	}
}
