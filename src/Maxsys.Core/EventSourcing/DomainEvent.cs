using Maxsys.Core.Messaging.Abstractions;
using MediatR;

namespace Maxsys.Core.EventSourcing;

public abstract record DomainEvent : IDomainEvent, INotification
{
    public DateTime Timestamp { get; private set; }
    public string MessageType { get; private set; }

    protected DomainEvent()
    {
        Timestamp = DateTime.UtcNow;
        MessageType = GetType().Name;
    }
}