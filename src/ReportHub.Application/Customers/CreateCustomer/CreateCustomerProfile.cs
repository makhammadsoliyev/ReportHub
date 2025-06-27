using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Customers.CreateCustomer;

public class CreateCustomerProfile : Profile
{
	public CreateCustomerProfile()
	{
		CreateMap<CreateCustomerRequest, Customer>();
	}
}
