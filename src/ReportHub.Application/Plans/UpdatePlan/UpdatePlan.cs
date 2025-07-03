using AutoMapper;
using FluentValidation;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Plans.UpdatePlan;

public class UpdatePlanCommand(UpdatePlanRequest plan, Guid organizationId)
	: ICommand<Guid>, IOrganizationRequest
{
	public UpdatePlanRequest Plan { get; set; } = plan;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin)]
public class UpdatePlanCommandHandler(
	IMapper mapper,
	ICountryService service,
	IPlanRepository planRepository,
	IValidator<UpdatePlanRequest> validator,
	IOrganizationRepository organizationRepository)
	: ICommandHandler<UpdatePlanCommand, Guid>
{
	public async Task<Guid> Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(request.Plan, cancellationToken);

		var isDuplicateTitle = await planRepository.AnyAsync(
			t => t.Title.ToLower() == request.Plan.Title.ToLower() && t.Id == request.Plan.Id);
		if (isDuplicateTitle)
		{
			throw new ConflictException($"Plan is already exist with this title: {request.Plan.Title}");
		}

		var organization = await organizationRepository.SelectAsync(t => t.Id == request.OrganizationId);
		var plan = await planRepository.SelectAsync(t => t.Id == request.Plan.Id)
			?? throw new NotFoundException($"Plan is not found with this id: {request.Plan.Id}");
		mapper.Map(request.Plan, plan);
		plan.OrganizationId = request.OrganizationId;
		plan.CurrencyCode = await service.GetCurrencyCodeByCountryCodeAsync(organization.CountryCode);
		await planRepository.UpdateAsync(plan);

		return plan.Id;
	}
}
