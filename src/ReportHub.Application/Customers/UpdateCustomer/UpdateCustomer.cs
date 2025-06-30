using AutoMapper;
using FluentValidation;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Customers.UpdateCustomer;

public class UpdateCustomerCommand(UpdateCustomerRequest customer, Guid organizationId)
	: ICommand<Guid>, IOrganizationRequest
{
	public UpdateCustomerRequest Customer { get; set; } = customer;

	public Guid OrganizationId { get; set; } = organizationId;
}

public class UpdateCustomerCommandHandler(
	IMapper mapper,
	ICustomerRepository repository,
	IValidator<UpdateCustomerRequest> validator)
	: ICommandHandler<UpdateCustomerCommand, Guid>
{
	public async Task<Guid> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(request.Customer, cancellationToken);

		var customer = await repository.SelectAsync(t => t.Id == request.Customer.Id)
			?? throw new NotFoundException($"Customer is not found with this: {request.Customer.Id}");
		customer.OrganizationId = request.OrganizationId;
		mapper.Map(request.Customer, customer);

		return customer.Id;
	}
}
