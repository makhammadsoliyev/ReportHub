using System.Reflection;
using MediatR;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>(
	ICurrentOrganizationService currentOrganizationService,
	IRequestHandler<TRequest, TResponse> handler,
	IOrganizationMemberRepository repository,
	ICurrentUserService currentUserService)
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>, IBaseRequest
{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var requiredUserRoles = handler.GetType().GetCustomAttribute<RequiresUserRoleAttribute>()?.Roles ?? [];
		var requiredOrganizationRoles = handler.GetType().GetCustomAttribute<RequiresOrganizationRoleAttribute>()?.Roles ?? [];

		if (requiredUserRoles is [] && requiredOrganizationRoles is [])
		{
			return await next(cancellationToken);
		}

		if (request is IOrganizationRequest organizationRequest)
		{
			currentOrganizationService.OrganizationId = organizationRequest.OrganizationId;

			var organizationRoles = await repository.SelectRoleAsync(currentUserService.UserId);
			if (requiredOrganizationRoles.Intersect(organizationRoles).Any())
			{
				return await next(cancellationToken);
			}
		}

		var userRoles = currentUserService.UserRoles;
		if (requiredUserRoles.Intersect(userRoles).Any())
		{
			return await next(cancellationToken);
		}

		throw new ForbiddenException("You are not allowed to perform this action");
	}
}
