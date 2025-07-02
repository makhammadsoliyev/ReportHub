using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Invoices.GetInvoiceItemList;

public class GetInvoiceItemListProfile : Profile
{
	public GetInvoiceItemListProfile()
	{
		CreateMap<InvoiceItem, GetInvoiceItemListDto>();
	}
}
