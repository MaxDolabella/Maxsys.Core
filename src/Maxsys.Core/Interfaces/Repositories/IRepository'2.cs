using System.Linq.Expressions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Interfaces.Repositories;

/// <summary>
/// Fornece uma interface para um repositório da entidade <typeparamref name="TEntity"/>.<br/>
/// <para/>Aviso - "Sempre prefira Composição a Herança": <see href="https://youtu.be/LfiezdBs318?t=890"/>
/// </summary>
/// <typeparam name="TEntity">é a entidade do banco.</typeparam>
/// <typeparam name="TFilter">é o tipo do filtro para ser usado nas consultas do banco.</typeparam>
public interface IRepository<TEntity, TFilter> : IRepository<TEntity>
    where TEntity : class
    where TFilter : IFilter<TEntity>
{
    #region QTD

    /// <summary>NEW</summary>
    ValueTask<int> CountAsync(TFilter filters, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    ValueTask<bool> AnyAsync(TFilter filters, CancellationToken cancellationToken = default);

    #endregion QTD

    #region LIST

    /// <summary>NEW</summary>
    Task<IReadOnlyList<TEntity>> GetListAsync(TFilter filters, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    Task<IReadOnlyList<TEntity>> GetListAsync(TFilter filters, ListCriteria criteria, ISortColumnSelector<TEntity> sortColumnSelector, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    Task<IReadOnlyList<TEntity>> GetListAsync(TFilter filters, Pagination? pagination, Expression<Func<TEntity, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascendant, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    Task<IReadOnlyList<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>NEW</summary>
    Task<IReadOnlyList<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>>? predicate, ListCriteria criteria, ISortColumnSelector<TDestination> sortColumnSelector, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>NEW</summary>
    Task<IReadOnlyList<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>>? predicate, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascendant, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>NEW</summary>
    Task<IReadOnlyList<TDestination>> GetListAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>NEW</summary>
    Task<IReadOnlyList<TDestination>> GetListAsync<TDestination>(TFilter filters, ListCriteria criteria, ISortColumnSelector<TDestination> sortColumnSelector, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>NEW</summary>
    Task<IReadOnlyList<TDestination>> GetListAsync<TDestination>(TFilter filters, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascendant, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion LIST

    #region GET

    /// <summary>NEW</summary>
    Task<TEntity?> GetAsync(TFilter filters, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    Task<TEntity?> GetAsync<TProperty>(TFilter filters, Expression<Func<TEntity, TProperty>> includeNavigation, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>NEW</summary>
    Task<TDestination?> GetAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion GET
}