using MediatR;

namespace Maxsys.Core.Messaging.Abstractions;

public interface IDomainEvent : INotification
{
    DateTime Timestamp { get; }
    string MessageType { get; }
}