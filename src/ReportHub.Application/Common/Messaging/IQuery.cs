using MediatR;

namespace ReportHub.Application.Common.Messaging;

public interface IQuery<T> : IRequest<T> where T : class;
