using AutoMapper;
using FluentValidation;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;
using ReportHub.Domain;

namespace ReportHub.Application.Items.CreateItem;

public class CreateItemCommand(CreateItemRequest item, Guid organizationId) : ICommand<Guid>, IOrganizationRequest
{
	public CreateItemRequest Item { get; set; } = item;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin)]
public class CreateItemCommandHandler(
	IMapper mapper,
	ICurrencyService service,
	IItemRepository repository,
	IValidator<CreateItemRequest> validator) : ICommandHandler<CreateItemCommand, Guid>
{
	public async Task<Guid> Handle(CreateItemCommand request, CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(request.Item, cancellationToken);

		var isValidCurrencyCode = await service.VerifyByAlphaCodeAsync(request.Item.CurrencyCode);
		if (!isValidCurrencyCode)
		{
			throw new BadRequestException("Invalid Currency Code");
		}

		var isDuplicateName = await repository.AnyAsync(t => t.Name.ToLower() == request.Item.Name.ToLower());
		if (isDuplicateName)
		{
			throw new ConflictException($"Item is already exist with this name: {request.Item.Name}");
		}

		var item = mapper.Map<Item>(request.Item);
		item.OrganizationId = request.OrganizationId;
		await repository.InsertAsync(item);

		return item.Id;
	}
}
