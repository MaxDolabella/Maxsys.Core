using System.Reflection;
using Maxsys.Excel.Abstractions;
using Maxsys.Excel.Infra;
using Maxsys.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Maxsys.Excel.Extensions;

public static class IoCExtensions
{
    /// <summary>
    /// Adiciona as implementações dos mapeamentos de excel para objeto (<see cref="TableTypeConfigurationBase{T}"/>)
    /// no <see cref="IServiceCollection"/> a partir do assembly ao qual <typeparamref name="TEntry"/> faz parte.
    /// </summary>
    public static IServiceCollection AddTableTypeConfigurations<TEntry>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.AddTableTypeConfigurations([typeof(TEntry).Assembly], serviceLifetime);

        return services;
    }

    /// <summary>
    /// Adiciona as implementações dos mapeamentos de excel para objeto (<see cref="TableTypeConfigurationBase{T}"/>)
    /// no <see cref="IServiceCollection"/> a partir de um array de assembly.
    /// </summary>
    public static IServiceCollection AddTableTypeConfigurations(this IServiceCollection services, Assembly[] assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var types = assemblies.SelectMany((a) => a.ExportedTypes)
            .Where(t => !t.GetCustomAttributes<DependencyInjectionIgnoreAttribute>().Any())
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.IsAssignableToGenericType(typeof(TableTypeConfigurationBase<>)))
            .ToList();

        foreach (var type in types)
        {
            services.Add(new ServiceDescriptor(type.BaseType!, type, serviceLifetime));
        }

        return services;
    }

    /// <summary>
    /// Adiciona os mapeamentos de objeto (<see cref="TableTypeConfigurationBase{T}"/>) e suas implementações no <see cref="IServiceCollection"/>.
    /// </summary>
    [Obsolete("Método será removido. Utilizar AddTableTypeConfigurations<>() ou AddTableTypeConfigurations(assemblies)", error: true)]
    public static IServiceCollection AddTableTypeConfiguration<TImplementation, T>(this IServiceCollection services)
        where TImplementation : TableTypeConfigurationBase<T>
    {
        services.AddScoped<TableTypeConfigurationBase<T>, TImplementation>();

        return services;
    }

    /// <summary>
    /// Adiciona uma implementação de <see cref="IWorkbookFacade"/> no <see cref="IServiceCollection"/>.
    /// </summary>
    public static IServiceCollection AddWorkbookService<TService, TImplementation>(this IServiceCollection services)
        where TService : IWorkbookFacade
        where TImplementation : class, TService
    {
        services.AddScoped<IWorkbookFacade, TImplementation>();

        return services;
    }

    /// <summary>
    /// Adiciona uma implementação de <see cref="IWorkbookFacade"/> no <see cref="IServiceCollection"/>.
    /// </summary>
    public static IServiceCollection AddWorkbookService<TImplementation>(this IServiceCollection services)
        where TImplementation : class, IWorkbookFacade
    {
        services.AddScoped<TImplementation>();

        return services;
    }
}