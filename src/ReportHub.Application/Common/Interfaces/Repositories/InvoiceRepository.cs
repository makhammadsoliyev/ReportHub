using System.Linq.Expressions;
using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface IInvoiceRepository
{
	Task InsertAsync(Invoice invoice);

	Task UpdateAsync(Invoice invoice);

	Task<bool> DeleteAsync(Invoice invoice);

	Task<Invoice> SelectAsync(Expression<Func<Invoice, bool>> expression);
}
