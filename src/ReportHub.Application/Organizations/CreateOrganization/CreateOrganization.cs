using AutoMapper;
using FluentValidation;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;
using ReportHub.Domain;

namespace ReportHub.Application.Organizations.CreateOrganization;

public class CreateOrganizationCommand(string name, string countryCode, Guid userId) : ICommand<Guid>
{
	public string Name { get; set; } = name;

	public string CountryCode { get; set; } = countryCode;

	public Guid UserId { get; set; } = userId;
}

[RequiresUserRole(UserRoles.Admin)]
public class CreateOrganizationCommandHandler(
	IMapper mapper,
	ICountryService service,
	IUserRepository userRepository,
	IOrganizationRepository organizationRepository,
	IOrganizationRoleRepository organizationRoleRepository,
	IOrganizationMemberRepository organizationMemberRepository,
	IValidator<CreateOrganizationCommand> validator) : ICommandHandler<CreateOrganizationCommand, Guid>
{
	public async Task<Guid> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(request, cancellationToken);

		var isValidCountryCode = await service.VerifyByAlphaCodeAsync(request.CountryCode);
		if (!isValidCountryCode)
		{
			throw new BadRequestException("Invalid Country Code");
		}

		var isNameUnique = !await organizationRepository.AnyAsync(t => t.Name.ToLower() == request.Name.ToLower());
		if (!isNameUnique)
		{
			throw new ConflictException($"Organization already exists with this name: {request.Name}");
		}

		var isUserExist = await userRepository.AnyAsync(t => t.Id == request.UserId);
		if (!isUserExist)
		{
			throw new NotFoundException($"User is not found with this id: {request.UserId}");
		}

		var organization = mapper.Map<Organization>(request);
		await organizationRepository.InsertAsync(organization);
		await organizationMemberRepository.InsertAsync(new OrganizationMember
		{
			OrganizationId = organization.Id,
			UserId = request.UserId,
			OrganizationRole = await organizationRoleRepository.SelectAsync(t => t.Name == OrganizationRoles.Owner),
		});

		return organization.Id;
	}
}
