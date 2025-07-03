using AutoMapper;
using FluentValidation;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;
using ReportHub.Domain;

namespace ReportHub.Application.Plans.CreatePlan;

public class CreatePlanCommand(CreatePlanRequest plan, Guid organizationId)
	: ICommand<Guid>, IOrganizationRequest
{
	public CreatePlanRequest Plan { get; set; } = plan;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin)]
public class CreatePlanCommandHandler(
	IMapper mapper,
	ICountryService service,
	IPlanRepository planRepository,
	IValidator<CreatePlanRequest> validator,
	IOrganizationRepository organizationRepository)
	: ICommandHandler<CreatePlanCommand, Guid>
{
	public async Task<Guid> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(request.Plan, cancellationToken);

		var isDuplicateTitle = await planRepository.AnyAsync(t => t.Title.ToLower() == request.Plan.Title.ToLower());
		if (isDuplicateTitle)
		{
			throw new ConflictException($"Plan is already exist with this title: {request.Plan.Title}");
		}

		var plan = mapper.Map<Plan>(request.Plan);
		var organization = await organizationRepository.SelectAsync(t => t.Id == request.OrganizationId);
		plan.OrganizationId = request.OrganizationId;
		plan.CurrencyCode = await service.GetCurrencyCodeByCountryCodeAsync(organization.CountryCode);
		await planRepository.InsertAsync(plan);

		return plan.Id;
	}
}
