using MediatR;

namespace ReportHub.Application.Common.Messaging;

public interface ICommand<T> : IRequest<T>;
