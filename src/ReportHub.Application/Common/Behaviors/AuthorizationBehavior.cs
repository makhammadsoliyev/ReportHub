using System.Reflection;
using MediatR;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Services;

namespace ReportHub.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler, ICurrentUserService currentUserService)
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>, IBaseRequest
{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var requiredUserRoles = handler.GetType().GetCustomAttribute<RequiresUserRoleAttribute>()?.Roles;

		if (requiredUserRoles is null or [])
		{
			return await next(cancellationToken);
		}

		var userRoles = currentUserService.UserRoles;

		if (!requiredUserRoles.Intersect(userRoles).Any())
		{
			throw new ForbiddenException("You are not allowed to perform this action");
		}

		return await next(cancellationToken);
	}
}
