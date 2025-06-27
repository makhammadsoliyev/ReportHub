using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Customers.GetCustomersList;

public class GetCustomersListQuery : IQuery<IEnumerable<GetCustomersListDto>>
{
}

public class GetCustomersListQueryHandler(IMapper mapper, ICustomerRepository repository)
	: IQueryHandler<GetCustomersListQuery, IEnumerable<GetCustomersListDto>>
{
	public async Task<IEnumerable<GetCustomersListDto>> Handle(GetCustomersListQuery request, CancellationToken cancellationToken)
	{
		var customers = await repository
			.SelectAll()
			.ProjectTo<GetCustomersListDto>(mapper.ConfigurationProvider)
			.ToListAsync(cancellationToken);

		return customers;
	}
}

public class GetCustomersListProfile : Profile
{
	public GetCustomersListProfile()
	{

	}
}