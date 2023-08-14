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

    /// <summary>
    /// Add an object of type <typeparamref name="TEntity"/> in the repository asynchronously.
    /// </summary>
    /// <param name="entity">Is the <typeparamref name="TEntity"/> to add.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is added,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds  an <see cref="IEnumerable{T}"/> in the repository asynchronously, where T is <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="entities">Is the <see cref="IEnumerable{T}"/> of <typeparamref name="TEntity"/> to add.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is the <see cref="IEnumerable{T}"/> of <typeparamref name="TEntity"/> is added,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete an object of type <typeparamref name="TEntity"/> from the repository asynchronously.
    /// </summary>
    /// <param name="id">Is the key of the <typeparamref name="TEntity"/> to remove.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is deleted,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> DeleteAsync(object id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete an object of type <typeparamref name="TEntity"/> from the repository asynchronously.
    /// </summary>
    /// <param name="compositeKey">Is the composite key of the <typeparamref name="TEntity"/> to remove.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is deleted,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> DeleteAsync(object[] compositeKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update an object of type <typeparamref name="TEntity"/> in the repository asynchronously.
    /// </summary>
    /// <param name="entity">Is the <typeparamref name="TEntity"/> to update.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is updated,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update an object of type <typeparamref name="TEntity"/> in the repository asynchronously.
    /// </summary>
    /// <param name="entities">Is the <see cref="IEnumerable{T}"/> of <typeparamref name="TEntity"/> to update.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is updated,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    #endregion MOD

    #region QTD

    /// <summary>
    /// Asynchronously returns the number of elements in a sequence that satisfy a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    ValueTask<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously determines whether a sequence contains any elements that satisfy a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

    #endregion QTD

    #region LIST

    /// <summary>
    /// Get all items from the repository that matches a condition asynchronously.
    /// </summary>
    /// <param name="predicate">Is the condition to select the items.</param>
    /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, specifies wether entity must be tracked.
    /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
    /// <para/>Default is <see langword="true"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>an <see cref="IEnumerable{TEntity}"/> with all entities in repository.</returns>
    Task<IReadOnlyList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, bool @readonly = true, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate, ListCriteria criteria, ISortColumnSelector<TEntity> sortColumnSelector, bool @readonly = true, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate, Pagination? pagination, Expression<Func<TEntity, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascendant, bool @readonly = true, CancellationToken cancellationToken = default);

    #endregion LIST

    #region GET

    /// <summary>
    /// Get the first item from the repository that matches a condition asynchronously.
    /// </summary>
    /// <param name="predicate">Is the condition to select the items.</param>
    /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, specifies wether entity must be tracked.
    /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
    /// <para/>Default is <see langword="true"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>an <typeparamref name="TEntity"/> from the repository.</returns>
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true, CancellationToken cancellationToken = default);

    Task<TEntity?> GetAsync<TProperty>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProperty>> includeNavigation, bool @readonly = true, CancellationToken cancellationToken = default);

    #endregion GET
}