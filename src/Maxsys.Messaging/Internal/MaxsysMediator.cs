using System.Collections.Concurrent;
using System.Reflection;
using Maxsys.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Maxsys.Messaging.Internal;

/// <summary>
/// Implementação padrão do IMessageDispatcher.
/// Resolve handlers e behaviors via IServiceProvider e executa o pipeline.
/// </summary>
internal sealed class MaxsysMediator : IMessageDispatcher
{
    private readonly IServiceProvider _sp;

    private static readonly ConcurrentDictionary<(Type, Type), MethodInfo> _commandMethodCache = new();
    private static readonly ConcurrentDictionary<Type, MethodInfo> _voidCommandMethodCache = new();
    private static readonly ConcurrentDictionary<(Type, Type), MethodInfo> _queryMethodCache = new();

    public MaxsysMediator(IServiceProvider sp) => _sp = sp;

    // ── Commands com retorno ──────────────────────────────────────────────────

    public Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken ct)
    {
        var method = _commandMethodCache.GetOrAdd(
            (command.GetType(), typeof(TResponse)),
            k => typeof(MaxsysMediator)
                .GetMethod(nameof(SendCommandCoreAsync), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(k.Item1, k.Item2));

        return (Task<TResponse>)method.Invoke(this, [command, ct])!;
    }

    private async Task<TResponse> SendCommandCoreAsync<TCommand, TResponse>(TCommand command, CancellationToken ct)
        where TCommand : ICommand<TResponse>
    {
        var handler = _sp.GetRequiredService<ICommandHandler<TCommand, TResponse>>();
        var behaviors = _sp.GetServices<IPipelineBehavior<TCommand, TResponse>>().Reverse().ToList();

        Func<Task<TResponse>> pipeline = () => handler.HandleAsync(command, ct);

        foreach (var behavior in behaviors)
        {
            var next = pipeline;
            var b = behavior;
            pipeline = () => b.HandleAsync(command, next, ct);
        }

        return await pipeline();
    }

    // ── Commands sem retorno ──────────────────────────────────────────────────

    public Task SendAsync(ICommand command, CancellationToken ct)
    {
        var method = _voidCommandMethodCache.GetOrAdd(
            command.GetType(),
            t => typeof(MaxsysMediator)
                .GetMethod(nameof(SendVoidCommandCoreAsync), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(t));

        return (Task)method.Invoke(this, [command, ct])!;
    }

    private async Task SendVoidCommandCoreAsync<TCommand>(TCommand command, CancellationToken ct)
        where TCommand : ICommand
    {
        var handler = _sp.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.HandleAsync(command, ct);
    }

    // ── Queries ───────────────────────────────────────────────────────────────

    public Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken ct)
    {
        var method = _queryMethodCache.GetOrAdd(
            (query.GetType(), typeof(TResponse)),
            k => typeof(MaxsysMediator)
                .GetMethod(nameof(SendQueryCoreAsync), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(k.Item1, k.Item2));

        return (Task<TResponse>)method.Invoke(this, [query, ct])!;
    }

    private async Task<TResponse> SendQueryCoreAsync<TQuery, TResponse>(TQuery query, CancellationToken ct)
        where TQuery : IQuery<TResponse>
    {
        var handler = _sp.GetRequiredService<IQueryHandler<TQuery, TResponse>>();
        var behaviors = _sp.GetServices<IPipelineBehavior<TQuery, TResponse>>().Reverse().ToList();

        Func<Task<TResponse>> pipeline = () => handler.HandleAsync(query, ct);

        foreach (var behavior in behaviors)
        {
            var next = pipeline;
            var b = behavior;
            pipeline = () => b.HandleAsync(query, next, ct);
        }

        return await pipeline();
    }

    // ── Events (broadcast) ────────────────────────────────────────────────────

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct)
        where TEvent : class, IEvent
    {
        var handlers = _sp.GetServices<IEventHandler<TEvent>>();
        await Task.WhenAll(handlers.Select(h => h.HandleAsync(@event, ct)));
    }
}