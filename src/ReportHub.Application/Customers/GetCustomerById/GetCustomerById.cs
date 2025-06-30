using AutoMapper;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Customers.GetCustomerById;

public class GetCustomerByIdQuery(Guid id, Guid organizationId)
	: IQuery<GetCustomerByIdDto>, IOrganizationRequest
{
	public Guid Id { get; set; } = id;

	public Guid OrganizationId { get; set; } = organizationId;
}

public class GetCustomerByIdQueryHandler(IMapper mapper, ICustomerRepository repository)
	: IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdDto>
{
	public async Task<GetCustomerByIdDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
	{
		var customer = await repository.SelectAsync(t => t.Id == request.Id)
			?? throw new NotFoundException($"Customer is not found with this id: {request.Id}");

		return mapper.Map<GetCustomerByIdDto>(customer);
	}
}
