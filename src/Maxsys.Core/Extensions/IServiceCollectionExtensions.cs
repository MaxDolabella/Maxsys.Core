﻿using System.Reflection;
using FluentValidation;
using Maxsys.Core.Helpers;
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
    /// Registra as implementações das interfaces públicas que herdam da interface <typeparamref name="TInterface"/>.<br/>
    /// As interfaces herdadas de <typeparamref name="TInterface"/> são buscados nos assemblies <paramref name="interfaceAssemblies"/>.<br/>
    /// As implementações dessas interfaces são buscadas nos assemblies <paramref name="implementationAssemblies"/>.<br/>
    /// Caso <paramref name="suffix"/> diferente de nulo, somente interfaces e implementações terminadas com esse sufixo serão buscadas.
    /// </summary>
    /// <remarks>ATENÇÃO: Interfaces sem implementações serão ignoradas.</remarks>
    /// <typeparam name="TInterface">O tipo do serviço a ser registrado.</typeparam>
    /// <param name="services">o <see cref="IServiceCollection"/> onde serão adicionadas as interfaces e suas implementações.</param>
    /// <param name="interfaceAssemblies">os assemblies de onde serão obtidas as interfaces.</param>
    /// <param name="implementationAssemblies">os assemblies de onde serão obtidas as implementações.</param>
    /// <param name="suffix">O sufixo das interfaces e implementações a serem registrados.</param>
    /// <returns>A mesma instância de <paramref name="services"/> após a operação completada.</returns>
    public static IServiceCollection AddImplementations<TInterface>(this IServiceCollection services, Assembly[] interfaceAssemblies, Assembly[] implementationAssemblies, string? suffix = null)
        where TInterface : class
    {
        var implementationDictionary = ReflectionHelper.GetImplementationDictionary<TInterface>(interfaceAssemblies, implementationAssemblies, suffix);

        return RegisterImplementationDictionary(services, implementationDictionary);
    }

    /// <summary>
    /// Registra as implementações das interfaces públicas que herdam da interface <typeparamref name="TInterface"/>.<br/>
    /// As interfaces herdadas de <typeparamref name="TInterface"/> são buscados no assembly onde está o tipo <typeparamref name="TInterfaceEntry"/>.<br/>
    /// As implementações dessas interfaces são buscadas no assembly onde está o tipo <typeparamref name="TImplementationEntry"/>.<br/>
    /// Caso <paramref name="suffix"/> diferente de nulo, somente interfaces e implementações terminadas com esse sufixo serão buscadas.
    /// </summary>
    /// <remarks>ATENÇÃO: Interfaces sem implementações serão ignoradas.</remarks>
    /// <typeparam name="TInterface">O tipo do serviço a ser registrado.</typeparam>
    /// <typeparam name="TInterfaceEntry">O tipo de onde será obtido o assembly que contém as interfaces a serem registradas.</typeparam>
    /// <typeparam name="TImplementationEntry">O tipo de onde será obtido o assembly que contém as implementações a serem registradas.</typeparam>
    /// <param name="services">o <see cref="IServiceCollection"/> onde serão adicionadas as interfaces e suas implementações.</param>
    /// <param name="suffix">O sufixo das interfaces e implementações a serem registrados.</param>
    /// <returns>A mesma instância de <paramref name="services"/> após a operação completada.</returns>
    public static IServiceCollection AddImplementations<TInterface, TInterfaceEntry, TImplementationEntry>(this IServiceCollection services, string? suffix = null)
        where TInterface : class
    {
        var interfaceAssemblies = new[] { typeof(TInterfaceEntry).Assembly };
        var implementationAssemblies = new[] { typeof(TImplementationEntry).Assembly };

        return AddImplementations<TInterface>(services, interfaceAssemblies, implementationAssemblies, suffix);
    }

    /// <summary>
    /// Registra as implementações das interfaces públicas que herdam da interface <typeparamref name="TInterface"/>.<br/>
    /// As interfaces herdadas de <typeparamref name="TInterface"/> e suas implementações serão buscadas no assembly onde está o tipo <typeparamref name="TEntry"/>.<br/>
    /// Caso <paramref name="suffix"/> diferente de nulo, somente interfaces e implementações terminadas com esse sufixo serão buscadas.
    /// </summary>
    /// <remarks>ATENÇÃO: Interfaces sem implementações serão ignoradas.</remarks>
    /// <typeparam name="TInterface">O tipo do serviço a ser registrado.</typeparam>
    /// <typeparam name="TEntry">O tipo de onde será obtido o assembly que contém as interfaces e suas implementações a serem registradas.</typeparam>
    /// <param name="services">o <see cref="IServiceCollection"/> onde serão adicionadas as interfaces e suas implementações.</param>
    /// <param name="suffix">O sufixo das interfaces e implementações a serem registrados.</param>
    /// <returns>A mesma instância de <paramref name="services"/> após a operação completada.</returns>
    public static IServiceCollection AddImplementations<TInterface, TEntry>(this IServiceCollection services, string? suffix = null)
        where TInterface : class
    {
        return AddImplementations<TInterface, TEntry, TEntry>(services, suffix);
    }

    /// <summary>
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
            .Where(type
                => !type.IsInterface
                && !type.IsAbstract
                && !type.GetCustomAttributes<DependencyInjectionIgnoreAttribute>().Any()
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
    public static IServiceCollection RegisterImplementationDictionary(this IServiceCollection services, IReadOnlyDictionary<Type, Type> keyValues, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        foreach (var item in keyValues)
            services.Add(new ServiceDescriptor(item.Key, item.Value, serviceLifetime));

        return services;
    }
}