using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Customers.GetCustomersList;

public class GetCustomersListProfile : Profile
{
	public GetCustomersListProfile()
	{
		CreateMap<Customer, GetCustomersListDto>();
	}
}
