using MediatR;

namespace ReportHub.Application.Common.Messaging;

public interface IQuery<out T> : IRequest<T>
{
}
