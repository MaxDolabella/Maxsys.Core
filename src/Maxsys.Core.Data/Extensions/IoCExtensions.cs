using Maxsys.Core.Exceptions;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Maxsys.Core.Data.Extensions;

public static class IoCExtensions
{
    /// <summary>
    /// Register <typeparamref name="TContext"/> using native Dependency Injection
    /// </summary>
    public static IServiceCollection AddContext<TContext>(this IServiceCollection services) where TContext : DbContext
        => services.AddDbContext<TContext>();

    /// <summary>
    /// Registra <typeparamref name="TUnitOfWork"/>.
    /// </summary>
    /// <typeparam name="TUnitOfWork"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddUnitOfWork<TUnitOfWork>(this IServiceCollection services) where TUnitOfWork : class, IUnitOfWork
        => services.AddScoped<IUnitOfWork, TUnitOfWork>();

    public static IServiceCollection AddGenericRepositories<TInterfaceEntry, TImplementationEntry>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        return lifetime switch
        {
            ServiceLifetime.Singleton => services.AddSingleton(typeof(IRepository<>), typeof(RepositoryBase<>)),
            ServiceLifetime.Scoped => services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>)),
            ServiceLifetime.Transient => services.AddTransient(typeof(IRepository<>), typeof(RepositoryBase<>)),
            _ => throw new InvalidEnumArgumentException<ServiceLifetime>(lifetime, nameof(lifetime)),
        };
    }
}