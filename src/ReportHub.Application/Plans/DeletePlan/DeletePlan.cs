using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Plans.DeletePlan;

public class DeletePlanCommand(Guid id, Guid organizationId) : ICommand<bool>, IOrganizationRequest
{
	public Guid Id { get; set; } = id;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner)]
public class DeletePlanCommandHandler(IPlanRepository repository) : ICommandHandler<DeletePlanCommand, bool>
{
	public async Task<bool> Handle(DeletePlanCommand request, CancellationToken cancellationToken)
	{
		var plan = await repository.SelectAsync(t => t.Id == request.Id)
			?? throw new NotFoundException($"Plan is not found with id: {request.Id}");

		var result = await repository.DeleteAsync(plan);

		return result;
	}
}
