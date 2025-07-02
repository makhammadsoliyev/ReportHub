using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface IInvoiceItemRepository
{
	Task InsertAsync(InvoiceItem invoiceItem);

	Task<bool> DeleteAsync(InvoiceItem invoiceItem);

	Task<InvoiceItem> SelectAsync(Expression<Func<InvoiceItem, bool>> expression);

	IQueryable<InvoiceItem> SelectAll();
}
