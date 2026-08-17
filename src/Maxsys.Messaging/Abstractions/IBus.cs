namespace Maxsys.Messaging.Abstractions;

/// <summary>
/// Ponto de entrada para envio de mensagens. Injete IBus nos seus controllers e services.
/// </summary>
public interface IBus
{
    /// <summary>Publica um evento para todos os handlers registrados (broadcast em paralelo).</summary>
    Task Publish<TEvent>(TEvent @event, CancellationToken ct = default) where TEvent : class, IEvent;

    /// <summary>Envia uma query para o handler único registrado e retorna o resultado.</summary>
    Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken);

    /// <summary>Envia um command com retorno para o handler único registrado.</summary>
    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken);

    /// <summary>Envia um command sem retorno para o handler único registrado.</summary>
    Task SendAsync(ICommand command, CancellationToken cancellationToken);
}