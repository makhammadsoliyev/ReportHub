using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Invoices.UpdateInvoice;

public class UpdateInvoiceProfile : Profile
{
	public UpdateInvoiceProfile()
	{
		CreateMap<UpdateInvoiceRequest, Invoice>();
	}
}
