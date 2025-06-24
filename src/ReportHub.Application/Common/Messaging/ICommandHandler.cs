using MediatR;

namespace ReportHub.Application.Common.Messaging;

public interface ICommandHandler<in TCommand, TResponse>
	: IRequestHandler<TCommand, TResponse>
	where TCommand : ICommand<TResponse>;
