using System.Reflection;
using AutoMapper;
using Maxsys.Core.Interfaces.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Maxsys.Mapping;

/// <summary>
/// Extensões de registro (DI) do adaptador AutoMapper para as abstrações de mapeamento Maxsys.
/// </summary>
public static class IoCExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Registra o AutoMapper (com scan de <see cref="Profile"/>s no assembly de <typeparamref name="TEntry"/>)
        /// e os adaptadores <see cref="IObjectMapper"/>/<see cref="IQueryProjector"/> usados por
        /// <c>ModelServiceBase</c> e <c>RepositoryBase</c>.
        /// </summary>
        /// <typeparam name="TEntry">Tipo âncora do assembly a ser escaneado (ex.: <c>IApplicationEntry</c>).</typeparam>
        /// <param name="configure">Configuração adicional opcional do AutoMapper.</param>
        public IServiceCollection AddMaxsysAutoMapper<TEntry>(Action<IMapperConfigurationExpression>? configure = null)
            => services.AddMaxsysAutoMapper(configure, typeof(TEntry).Assembly);

        /// <summary>
        /// Registra o AutoMapper (com scan de <see cref="Profile"/>s nos assemblies informados)
        /// e os adaptadores <see cref="IObjectMapper"/>/<see cref="IQueryProjector"/>.
        /// </summary>
        /// <param name="assemblies">Assemblies a serem escaneados em busca de <see cref="Profile"/>s.</param>
        public IServiceCollection AddMaxsysAutoMapper(params Assembly[] assemblies)
            => services.AddMaxsysAutoMapper(configure: null, assemblies);

        /// <summary>
        /// Registra o AutoMapper (com configuração adicional e scan de <see cref="Profile"/>s
        /// nos assemblies informados) e os adaptadores <see cref="IObjectMapper"/>/<see cref="IQueryProjector"/>.
        /// </summary>
        /// <param name="configure">Configuração adicional opcional do AutoMapper.</param>
        /// <param name="assemblies">Assemblies a serem escaneados em busca de <see cref="Profile"/>s.</param>
        public IServiceCollection AddMaxsysAutoMapper(Action<IMapperConfigurationExpression>? configure, params Assembly[] assemblies)
        {
            services.AddAutoMapper(cfg => configure?.Invoke(cfg), assemblies);

            services.TryAddTransient<AutoMapperAdapter>();
            services.TryAddTransient<IObjectMapper>(static sp => sp.GetRequiredService<AutoMapperAdapter>());
            services.TryAddTransient<IQueryProjector>(static sp => sp.GetRequiredService<AutoMapperAdapter>());

            return services;
        }
    }
}
