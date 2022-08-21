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
}