using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Customers.UpdateCustomer;

public class UpdateCustomerProfile : Profile
{
	public UpdateCustomerProfile()
	{
		CreateMap<UpdateCustomerRequest, Customer>();
	}
}
