using Maxsys.Core.Extensions;
using Maxsys.Core.Messaging.Abstractions;

namespace Maxsys.Core.EventSourcing;

public sealed class StoredEvent
{
    public Guid Id { get; private set; }
    public DateTime Timestamp { get; private set; }
    public string EventType { get; private set; } = string.Empty;
    public string Data { get; private set; } = string.Empty;

    public StoredEvent()
    { }

    public static StoredEvent Create<T>(T @event)
        where T : class, IDomainEvent
    {
        ArgumentNullException.ThrowIfNull(@event, nameof(@event));

        var json = @event.ToJson()!;

        return new StoredEvent
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            EventType = @event.MessageType,
            Data = json
        };
    }
}