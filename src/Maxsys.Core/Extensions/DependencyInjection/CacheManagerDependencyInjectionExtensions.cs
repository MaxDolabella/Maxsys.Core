using Maxsys.Core.Caching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Maxsys.Core.Extensions.DependencyInjection;

public static class CacheManagerDependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adiciona os serviços de cache padrão ao contêiner de injeção de dependências.
        /// Registra o MemoryCache e a implementação padrão do ICacheManager.
        /// </summary>
        /// <returns>A mesma instância de <see cref="IServiceCollection"/> para permitir chamadas encadeadas.</returns>
        /// <remarks>
        /// Este método registra:
        /// - MemoryCache como serviço singleton
        /// - CacheManager como implementação singleton de ICacheManager
        ///
        /// Use este método quando quiser utilizar a implementação padrão do gerenciador de cache.
        /// </remarks>
        /// <example>
        /// <code>
        /// services.AddCacheManager();
        /// </code>
        /// </example>
        public IServiceCollection AddCacheManager()
        {
            services.AddMemoryCache();
            services.TryAddSingleton<ICacheManager, CacheManager>();

            return services;
        }

        /// <summary>
        /// Adiciona uma implementação personalizada do gerenciador de cache ao contêiner de injeção de dependências.
        /// </summary>
        /// <typeparam name="TService">O tipo da implementação personalizada que deve implementar <see cref="ICacheManager"/>.</typeparam>
        /// <returns>A mesma instância de <see cref="IServiceCollection"/> para permitir chamadas encadeadas.</returns>
        /// <remarks>
        /// Este método permite registrar uma implementação customizada de ICacheManager.
        /// O tipo TService deve ser uma classe que implementa a interface ICacheManager.
        /// O serviço será registrado como singleton apenas se ainda não houver uma implementação registrada.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Registrando uma implementação personalizada
        /// services.AddCachingManager&lt;CustomCacheManager&gt;();
        /// </code>
        /// </example>
        /// <exception cref="ArgumentException">Lançada quando TService não implementa corretamente ICacheManager.</exception>
        public IServiceCollection AddCacheManager<TService>()
            where TService : class, ICacheManager
        {
            services.TryAddSingleton<ICacheManager, TService>();

            return services;
        }
    }
}