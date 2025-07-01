using AutoMapper;
using FluentValidation;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;
using ReportHub.Domain;

namespace ReportHub.Application.Customers.CreateCustomer;

public class CreateCustomerCommand(CreateCustomerRequest customer, Guid organizationId)
	: ICommand<Guid>, IOrganizationRequest
{
	public CreateCustomerRequest Customer { get; set; } = customer;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin)]
public class CreateCustomerCommandHandler(
	IMapper mapper,
	ICountryService service,
	ICustomerRepository repository,
	IValidator<CreateCustomerRequest> validator)
	: ICommandHandler<CreateCustomerCommand, Guid>
{
	public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(request.Customer, cancellationToken);
		var isValidCountryCode = await service.VerifyByAlphaCodeAsync(request.Customer.CountryCode);
		if (!isValidCountryCode)
		{
			throw new BadRequestException("Invalid Country Code");
		}

		var customer = mapper.Map<Customer>(request.Customer);
		customer.OrganizationId = request.OrganizationId;
		await repository.InsertAsync(customer);

		return customer.Id;
	}
}
