using Maxsys.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Maxsys.Core.Data.Extensions;

/// <summary/>
public static class IoCExtensions
{
    /// <summary>
    /// Register <typeparamref name="TContext"/> using native Dependency Injection
    /// </summary>
    public static IServiceCollection AddContext<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        // Para quando se passar DbContext no CTOR.
        services.AddDbContext<TContext>();

        // Para quando se passar TContext no CTOR.
        services.AddScoped<TContext>();

        return services;
    }

    /// <summary>
    /// Registra <typeparamref name="TUnitOfWork"/>.
    /// </summary>
    /// <typeparam name="TUnitOfWork"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddUnitOfWork<TUnitOfWork>(this IServiceCollection services) where TUnitOfWork : class, IUnitOfWork
    {
        services.AddScoped<IUnitOfWork, TUnitOfWork>();

        return services;
    }

    /// <summary>
    /// Registra <typeparamref name="TUnitOfWork"/>.
    /// </summary>
    /// <typeparam name="TUnitOfWork"></typeparam>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    public static IServiceCollection AddUnitOfWork<TUnitOfWork>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped) where TUnitOfWork : class, IUnitOfWork
    {
        services.Add(new ServiceDescriptor(typeof(IUnitOfWork), typeof(TUnitOfWork), lifetime));

        return services;
    }
}