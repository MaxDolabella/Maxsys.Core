namespace Maxsys.Messaging.Abstractions;

/// <summary>
/// Marcador base para eventos publicados via IBus.Publish (broadcast para N handlers).
/// Use diretamente para eventos de aplicação sem semântica de domínio.
/// Para eventos de domínio DDD, use <c>IDomainEvent</c> de Maxsys.EventSourcing.
/// </summary>
public interface IEvent;
