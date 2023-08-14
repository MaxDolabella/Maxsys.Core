using System.Linq.Expressions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Fornece uma interface para obtenção de dados.<br/>
/// </summary>
public interface IReadService<TFilter> : IService
    where TFilter : IFilter
{
    /// <remarks>
    /// Mapeamento necessário: Entidade -> <typeparamref name="TDestination"/>
    /// </remarks>
    Task<TDestination?> GetAsync<TDestination>(object id, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de um <typeparamref name="TFilter"/>, sem paginação e sem ordenação.
    /// </summary>
    /// <remarks>Mapeamento necessário: Entidade -> <typeparamref name="TDestination"/></remarks>
    /// <typeparam name="TDestination"></typeparam>
    /// <typeparam name="TFilter"></typeparam>
    Task<IReadOnlyList<TDestination>> GetListAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de um <typeparamref name="TFilter"/>, com <see cref="ListCriteria"/>.
    /// </summary>
    /// <remarks>
    /// Mapeamento necessário: Entidade -> <typeparamref name="TDestination"/>
    /// </remarks>
    Task<ListDTO<TDestination>> GetListAsync<TDestination>(TFilter filters, ListCriteria criteria, ISortColumnSelector<TDestination>? sortColumnSelector = null, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de um <typeparamref name="TFilter"/>, com <see cref="ListCriteria"/>.
    /// </summary>
    /// <remarks>
    /// Mapeamento necessário: Entidade -> <typeparamref name="TDestination"/>
    /// </remarks>
    Task<ListDTO<TDestination>> GetListAsync<TDestination>(TFilter filters, ListCriteria criteria, Expression<Func<TDestination, dynamic>>? columnSelector = null, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma quantidade de objetos a partir de um filtro.
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> CountAsync(TFilter? filters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se existe alguma entidade a partir de um filtro.
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> AnyAsync(TFilter? filters, CancellationToken cancellationToken = default);
}