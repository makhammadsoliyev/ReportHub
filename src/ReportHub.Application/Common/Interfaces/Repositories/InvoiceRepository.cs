using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Repositories;

public interface IInvoiceRepository
{
	Task InsertAsync(Invoice invoice);
}
