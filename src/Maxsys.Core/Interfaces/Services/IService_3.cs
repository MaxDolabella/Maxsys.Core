using System.Linq.Expressions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Fornece uma interface básica para obtenção de dados.<br/>
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TFilter"></typeparam>
public interface IService<TEntity, TKey, TFilter> : IService<TEntity, TFilter>
    where TEntity : class
    where TKey : notnull
    where TFilter : IFilter<TEntity>
{
    #region GET

    Task<TDestination?> GetAsync<TDestination>(TKey id, CancellationToken cancellationToken = default);

    Task<TDestination?> GetAsync<TDestination>(TKey id, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);

    #endregion GET

    #region LIST

    Task<List<InfoDTO<TKey>>> ToInfoListAsync(Expression<Func<TEntity, bool>> predicate, ListCriteria criteria, CancellationToken cancellationToken = default);

    Task<List<InfoDTO<TKey>>> ToInfoListAsync(Expression<Func<TEntity, bool>> predicate, Pagination? pagination, Expression<Func<InfoDTO<TKey>, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    Task<List<InfoDTO<TKey>>> ToInfoListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<List<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default);

    Task<List<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, Pagination? pagination, Expression<Func<InfoDTO<TKey>, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    Task<List<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, CancellationToken cancellationToken = default);

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <see cref="InfoDTO{TKey}"/> </remarks>
    Task<List<InfoDTO<TKey>>> ToInfoListAsync(IEnumerable<TKey> idList, CancellationToken cancellationToken = default);

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <see cref="InfoDTO{TKey}"/> </remarks>
    Task<List<InfoDTO<TKey>>> ToInfoListAsync(IEnumerable<TKey> idList, ListCriteria criteria, CancellationToken cancellationToken = default);

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <see cref="InfoDTO{TKey}"/> </remarks>
    Task<List<InfoDTO<TKey>>> ToInfoListAsync(IEnumerable<TKey> idList, Pagination? pagination, Expression<Func<InfoDTO<TKey>, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/></remarks>
    Task<List<TDestination>> ToListAsync<TDestination>(IEnumerable<TKey> idList, CancellationToken cancellationToken = default);

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/></remarks>
    Task<List<TDestination>> ToListAsync<TDestination>(IEnumerable<TKey> idList, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/></remarks>
    Task<List<TDestination>> ToListAsync<TDestination>(IEnumerable<TKey> idList, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <see cref="InfoDTO{TKey}"/> </remarks>
    Task<ListDTO<InfoDTO<TKey>>> GetInfoListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default);

    #endregion LIST
}