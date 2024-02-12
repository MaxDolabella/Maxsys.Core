using Maxsys.Core.Excel.Abstractions;
using Maxsys.Core.Excel.Infra;
using Microsoft.Extensions.DependencyInjection;

namespace Maxsys.Core.Excel.Extensions;

public static class IoCExtensions
{
    /// <summary>
    /// Adiciona os mapeamentos de objeto (<see cref="TableTypeConfigurationBase{T}"/>) e suas implementações no <see cref="IServiceCollection"/>.
    /// </summary>
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