using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Invoices.CreateInvoice;

public class CreateInvoiceProfile : Profile
{
	public CreateInvoiceProfile()
	{
		CreateMap<CreateInvoiceRequest, Invoice>();
	}
}
