using Maxsys.DataCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Maxsys.Core.Data.Extensions.DependencyInjection;

public static class IoCExtensions
{
    /// <summary>
    /// Register <typeparamref name="TContext"/> using native Dependency Injection
    /// </summary>
    public static IServiceCollection AddContext<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        services.AddDbContext<TContext>();

        // Precisa ??
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
}