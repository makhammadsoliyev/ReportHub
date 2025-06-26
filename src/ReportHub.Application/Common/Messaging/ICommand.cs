using MediatR;

namespace ReportHub.Application.Common.Messaging;

public interface ICommand<out T> : IRequest<T>
{
}
