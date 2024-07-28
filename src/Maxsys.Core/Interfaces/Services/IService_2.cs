using System.Linq.Expressions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Fornece uma interface básica para obtenção de dados.<br/>
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TFilter"></typeparam>
public interface IService<TEntity, TFilter> : IService<TEntity>
    where TEntity : class
    where TFilter : IFilter<TEntity>
{
    #region GET

    Task<TDestination?> GetAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default);

    Task<TDestination?> GetAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);

    Task<TDestination?> GetSingleOrDefaultAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default);

    Task<TDestination?> GetSingleOrThrowsAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default);

    #endregion GET

    #region LIST

    Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default);

    Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);

    Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion LIST

    #region QTY

    /// <summary>
    /// Obtém uma quantidade de objetos a partir de um filtro.
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<int> CountAsync(TFilter? filters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se existe alguma entidade a partir de um filtro.
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<bool> AnyAsync(TFilter? filters, CancellationToken cancellationToken = default);

    #endregion QTY
}