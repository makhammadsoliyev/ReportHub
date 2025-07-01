using AutoMapper;
using FluentValidation;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Organizations.UpdateOrganization;

public class UpdateOrganizationCommand(Guid id, string name, string countryCode) : ICommand<Guid>
{
	public Guid Id { get; set; } = id;

	public string Name { get; set; } = name;

	public string CountryCode { get; set; } = countryCode;
}

[RequiresUserRole(UserRoles.Admin)]
public class UpdateOrganizationCommandHandler(
	IMapper mapper,
	IOrganizationRepository organizationRepository,
	IValidator<UpdateOrganizationCommand> validator) : ICommandHandler<UpdateOrganizationCommand, Guid>
{
	public async Task<Guid> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(request, cancellationToken);
		var isNameUnique = !await organizationRepository.AnyAsync(t => t.Name.ToLower() == request.Name.ToLower() && t.Id != request.Id);
		if (!isNameUnique)
		{
			throw new ConflictException($"Organization already exists with this name: {request.Name}");
		}

		var organization = await organizationRepository.SelectAsync(t => t.Id == request.Id)
			?? throw new NotFoundException($"Organization is not found with this id: {request.Id}");

		mapper.Map(request, organization);
		await organizationRepository.UpdateAsync(organization);

		return organization.Id;
	}
}
