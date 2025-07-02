using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Invoices.GetInvoicesList;

public class GetInvoicesListProfile : Profile
{
	public GetInvoicesListProfile()
	{
		CreateMap<Invoice, GetInvoicesListDto>();
	}
}
