using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Invoices.AddInvoiceItem;

public class AddInvoiceItemProfile : Profile
{
	public AddInvoiceItemProfile()
	{
		CreateMap<AddInvoiceItemRequest, InvoiceItem>();
	}
}
