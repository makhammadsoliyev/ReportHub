using Microsoft.AspNetCore.Http;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Customers.ImportCustomers;

public class ImportCustomersCommand(IFormFile file, Guid organizationId)
	: ICommand<int>, IOrganizationRequest
{
	public IFormFile File { get; set; } = file;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin)]
public class ImportCustomersCommandHandler(IAsposeService service, ICustomerRepository repository) : ICommandHandler<ImportCustomersCommand, int>
{
	public async Task<int> Handle(ImportCustomersCommand request, CancellationToken cancellationToken)
	{
		if (request.File == null || request.File.Length == 0)
		{
			throw new BadRequestException("No file uploaded");
		}

		using var stream = request.File.OpenReadStream();
		var customers = service.ImportCustomers(stream);

		var existingCustomers = repository.SelectAll();

		var importedCustomers = customers
			.Where(customer => !existingCustomers
				.Any(existCustomer =>
					existCustomer.Name == customer.Name &&
					existCustomer.Email == customer.Email &&
					existCustomer.CountryCode == customer.CountryCode))
			.ToList();

		importedCustomers.ForEach(customer => customer.OrganizationId = request.OrganizationId);

		await repository.InsertAsync(importedCustomers);

		return importedCustomers.Count;
	}
}
