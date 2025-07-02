using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Invoices.GetInvoiceById;

public class GetInvoiceByIdProfile : Profile
{
	public GetInvoiceByIdProfile()
	{
		CreateMap<Invoice, GetInvoiceByIdDto>();
	}
}
