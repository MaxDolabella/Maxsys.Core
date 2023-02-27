using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Maxsys.ModelCore.Sorting;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

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

    public static IServiceCollection AddSortSelectors<TEntry>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        var sortableColumns = GetClassesImplementingInterface(typeof(TEntry).Assembly, typeof(ISortColumnSelector<>));

        sortableColumns.ForEach(type =>
        {
            var implementedInterface = type.GetInterfaces()[0];
            // 'implementedInterface.GetGenericArguments()[0].Name' é o tipo genérico.

            var serviceDescriptor = new ServiceDescriptor(implementedInterface, type, lifetime);
            services.TryAdd(serviceDescriptor);
        });

        return services;
    }

    #region Private

    private static List<Type> GetClassesImplementingInterface(Assembly assembly, Type implementedInterface)
    {
        return GetClassesImplementingInterface(new[] { assembly }, implementedInterface);
    }

    private static List<Type> GetClassesImplementingInterface(Assembly[] assemblies, Type implementedInterface)
    {
        return assemblies.SelectMany(a => a.ExportedTypes)
            .Where(type => !type.IsInterface && !type.IsAbstract
                && type.GetInterfaces()
                    .Where(t => t.IsGenericType)
                    .Any(t => t.GetGenericTypeDefinition() == implementedInterface))
            .ToList();
    }

    #endregion Private
}