using MediatR;

namespace ReportHub.Application.Common.Messaging;

public interface ICommandHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : class;
