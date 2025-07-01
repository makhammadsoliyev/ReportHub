using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Customers.DeleteCustomer;

public class DeleteCustomerCommand(Guid id, Guid organizationId) : ICommand<bool>, IOrganizationRequest
{
	public Guid Id { get; set; } = id;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner)]
public class DeleteCustomerCommandHandler(ICustomerRepository repository)
	: ICommandHandler<DeleteCustomerCommand, bool>
{
	public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
	{
		var customer = await repository.SelectAsync(t => t.Id == request.Id)
			?? throw new NotFoundException($"Customer is not found with this id: {request.Id}");

		var result = await repository.DeleteAsync(customer);

		return result;
	}
}
