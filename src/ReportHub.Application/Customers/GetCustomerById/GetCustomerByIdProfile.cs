using AutoMapper;
using ReportHub.Application.Customers.GetCustomersList;
using ReportHub.Domain;

namespace ReportHub.Application.Customers.GetCustomerById;

public class GetCustomerByIdProfile : Profile
{
	public GetCustomerByIdProfile()
	{
		CreateMap<Customer, GetCustomersListDto>();
	}
}
