namespace Maxsys.Messaging.Abstractions;

/// <summary>
/// Intercepta o pipeline de execução de um request, permitindo executar lógica antes e depois do handler.
/// Registre implementações como open generic no DI: services.AddTransient(typeof(IPipelineBehavior&lt;,&gt;), typeof(MeuBehavior&lt;,&gt;))
/// </summary>
public interface IPipelineBehavior<TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken ct);
}