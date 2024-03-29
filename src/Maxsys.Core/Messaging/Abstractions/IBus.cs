namespace Maxsys.Core.Messaging.Abstractions;

public interface IBus
{
    Task Publish<TEvent>(TEvent @event) where TEvent : class, IDomainEvent;

    Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken);

    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken);

    Task SendAsync(ICommand command, CancellationToken cancellationToken);
}