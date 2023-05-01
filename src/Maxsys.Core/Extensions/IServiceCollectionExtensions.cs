using System;
using System.Collections.Generic;
using System.Linq;
using Maxsys.Core.Sorting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Maxsys.Core.Extensions;

/// <summary>
/// Provides extension methods to <see cref="IServiceCollection"/>
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Maxsys extension method.<br/>
    /// Adds a service of the type specified in <typeparamref name="TService"/> with an
    /// implementation type specified in <typeparamref name="TImplementation"/>
    /// with the specified <see cref="ServiceLifetime"/> to the
    /// specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection Add<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime));

        return services;
    }

    /// <summary>
    /// Maxsys extension method.<br/>
    /// Adds a service of the type specified in <typeparamref name="TService"/>
    /// with the specified <see cref="ServiceLifetime"/>
    /// to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection Add<TService>(this IServiceCollection services, ServiceLifetime lifetime)
        where TService : class
    {
        services.Add(new ServiceDescriptor(typeof(TService), lifetime));

        return services;
    }

    /// <summary>
    /// Registra as dependências de <see cref="ISortColumnSelector{T}"/> para suas implementações.
    /// </summary>
    /// <typeparam name="TEntry">Um tipo para servir de referência a fim de se obter
    /// os <see cref="ISortColumnSelector{T}"/> de seu Assembly.</typeparam>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    public static IServiceCollection AddSortSelectors<TEntry>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        var sortableColumns = typeof(TEntry).Assembly.ExportedTypes
            .Where(type => !type.IsInterface && !type.IsAbstract
                && type.GetInterfaces()
                    .Where(t => t.IsGenericType)
                    .Any(t => t.GetGenericTypeDefinition() == typeof(ISortColumnSelector<>)))
            .ToList();

        sortableColumns.ForEach(type =>
        {
            var implementedInterface = type.GetInterfaces()[0];
            // 'implementedInterface.GetGenericArguments()[0].Name' é o tipo genérico.

            var serviceDescriptor = new ServiceDescriptor(implementedInterface, type, lifetime);
            services.TryAdd(serviceDescriptor);
        });

        return services;
    }

    /// <summary>
    /// Registra os itens do dicionário onde Key é a interface e Value é a implementação.
    /// <para/>
    /// <example>Exemplo de uso:
    /// <code>
    /// RegisterDictionary(services, ReflectionHelper.GetImplementationDictionary&lt;IService&gt;(new[] { typeof(Entry).Assembly }, "Service"));
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="keyValues"></param>
    /// <param name="serviceLifetime"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterDictionary(this IServiceCollection services, IReadOnlyDictionary<Type, Type> keyValues, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        foreach (var item in keyValues)
            services.Add(new ServiceDescriptor(item.Key, item.Value, serviceLifetime));

        return services;
    }
}