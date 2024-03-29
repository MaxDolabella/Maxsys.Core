using Maxsys.Core.Messaging.Abstractions;

namespace Maxsys.Core.EventSourcing.Abstractions;

public interface IEventStore
{
    Task SaveAsync<T>(T @event) where T : class, IDomainEvent;
}