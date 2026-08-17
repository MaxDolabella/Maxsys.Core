using System.Reflection;
using Maxsys.Messaging.Abstractions;
using Maxsys.Messaging.Internal;
using Maxsys.Messaging.Pipelines;
using Microsoft.Extensions.DependencyInjection;

namespace Maxsys.Messaging.Extensions;

public static class MessagingServiceCollectionExtensions
{
    /// <summary>
    /// Registra o mediador Maxsys e todos os handlers do assembly de <typeparamref name="TEntry"/>.
    /// Inclui ValidationBehavior automaticamente. Use <paramref name="configure"/> para behaviors extras.
    /// </summary>
    public static IServiceCollection AddMessaging<TEntry>(
        this IServiceCollection services,
        Action<MessagingOptions>? configure = null)
        => AddMessaging(services, configure, typeof(TEntry).Assembly);

    /// <summary>
    /// Registra o mediador Maxsys e todos os handlers dos assemblies informados.
    /// Inclui ValidationBehavior automaticamente. Use <paramref name="configure"/> para behaviors extras.
    /// </summary>
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        Action<MessagingOptions>? configure = null,
        params Assembly[] assemblies)
    {
        AddMessagingCore(services, configure, assemblies);
        services.AddScoped<IBus, MaxsysBus>();
        return services;
    }

    /// <summary>
    /// Registra o mediador Maxsys com um IBus customizado (ex: wrapper de outra lib).
    /// O <paramref name="busFactory"/> substitui o MaxsysBus padrão.
    /// </summary>
    public static IServiceCollection AddMessaging<TEntry>(
        this IServiceCollection services,
        Func<IServiceProvider, IBus> busFactory,
        Action<MessagingOptions>? configure = null)
    {
        AddMessagingCore(services, configure, [typeof(TEntry).Assembly]);
        services.AddScoped(busFactory);
        return services;
    }

    private static void AddMessagingCore(IServiceCollection services, Action<MessagingOptions>? configure, Assembly[] assemblies)
    {
        var options = new MessagingOptions();
        configure?.Invoke(options);

        services.AddScoped<IMessageDispatcher, MaxsysMediator>();

        RegisterHandlers(services, assemblies);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        foreach (var behaviorType in options.BehaviorTypes)
            services.AddTransient(typeof(IPipelineBehavior<,>), behaviorType);
    }

    private static readonly Type[] HandlerInterfaces =
    [
        typeof(ICommandHandler<,>),
        typeof(ICommandHandler<>),
        typeof(IQueryHandler<,>),
        typeof(IEventHandler<>)
    ];

    private static void RegisterHandlers(IServiceCollection services, params Assembly[] assemblies)
        => RegisterHandlers(services, assemblies.SelectMany(a => a.GetTypes()));

    internal static void RegisterHandlers(IServiceCollection services, IEnumerable<Type> types)
    {
        var singleHandlers = new Dictionary<Type, Type>();

        foreach (var type in types)
        {
            if (!type.IsClass || type.IsAbstract || type.IsGenericTypeDefinition)
                continue;

            foreach (var iface in type.GetInterfaces())
            {
                if (!iface.IsGenericType)
                    continue;

                var genericDef = iface.GetGenericTypeDefinition();

                if (!HandlerInterfaces.Contains(genericDef))
                    continue;

                if (genericDef != typeof(IEventHandler<>))
                {
                    if (!singleHandlers.TryAdd(iface, type))
                    {
                        var messageType = iface.GetGenericArguments()[0].Name;
                        throw new InvalidOperationException(
                            $"Duplicate handler for '{messageType}'. Commands and Queries must have exactly one handler. " +
                            $"Conflicting types: '{singleHandlers[iface].Name}' and '{type.Name}'.");
                    }
                }

                services.AddScoped(iface, type);
            }
        }
    }
}