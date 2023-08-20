using System.Linq.Expressions;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Interfaces.Repositories;

/// <summary>
/// Fornece uma interface um repositório da entidade <typeparamref name="TEntity"/>.<br/>
/// Possui métodos básicos CRUD.
/// <para/>Aviso - "Sempre prefira Composição a Herança": <see href="https://youtu.be/LfiezdBs318?t=890"/>
/// </summary>
/// <typeparam name="TEntity">é a entidade do banco.</typeparam>
public interface IRepository<TEntity> : IRepository where TEntity : class
{
    #region MOD

    /// <summary>NEW</summary>
    ValueTask<bool> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    ValueTask<bool> AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    ValueTask<bool> DeleteAsync(object id, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    ValueTask<bool> DeleteAsync(object[] compositeKey, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    ValueTask<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    ValueTask<bool> UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    #endregion MOD

    #region QTD

    /// <summary>NEW</summary>
    ValueTask<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

    #endregion QTD

    #region LIST

    /// <summary>NEW</summary>
    Task<IReadOnlyList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    Task<IReadOnlyList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate, ListCriteria criteria, ISortColumnSelector<TEntity> sortColumnSelector, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    Task<IReadOnlyList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate, Pagination? pagination, Expression<Func<TEntity, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascendant, bool @readonly = true, CancellationToken cancellationToken = default);

    #endregion LIST

    #region GET

    /// <summary>NEW</summary>
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    Task<TEntity?> GetAsync<TProperty>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProperty>> includeNavigation, bool @readonly = true, CancellationToken cancellationToken = default);

    #endregion GET
}