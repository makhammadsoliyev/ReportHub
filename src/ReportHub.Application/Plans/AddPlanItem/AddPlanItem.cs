using AutoMapper;
using FluentValidation;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;
using ReportHub.Domain;

namespace ReportHub.Application.Plans.AddPlanItem;

public class AddPlanItemCommand(Guid planId, AddPlanItemRequest planItem, Guid organizationId)
	: ICommand<Guid>, IOrganizationRequest
{
	public Guid PlanId { get; set; } = planId;

	public AddPlanItemRequest PlanItem { get; set; } = planItem;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin)]
public class AddPlanItemCommandHandler(
	IMapper mapper,
	ICurrencyService service,
	IItemRepository itemRepository,
	IPlanRepository planRepository,
	IPlanItemRepository planItemRepository,
	IValidator<AddPlanItemRequest> validator) : ICommandHandler<AddPlanItemCommand, Guid>
{
	public async Task<Guid> Handle(AddPlanItemCommand request, CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(request.PlanItem, cancellationToken);

		var item = await itemRepository.SelectAsync(t => t.Id == request.PlanItem.ItemId)
			?? throw new NotFoundException($"Item is not found with this id: {request.PlanItem.ItemId}");

		var plan = await planRepository.SelectAsync(t => t.Id == request.PlanId)
			?? throw new NotFoundException($"Plan is not found with this id: {request.PlanId}");

		var planItem = mapper.Map<PlanItem>(request.PlanItem);
		planItem.OrganizationId = request.OrganizationId;
		planItem.CurrencyCode = plan.CurrencyCode;
		planItem.PlanId = request.PlanId;
		planItem.Price = await service.ExchangeAsync(item.CurrencyCode, plan.CurrencyCode, planItem.ItemsCount * item.Price);

		await planItemRepository.InsertAsync(planItem);

		return request.PlanId;
	}
}
