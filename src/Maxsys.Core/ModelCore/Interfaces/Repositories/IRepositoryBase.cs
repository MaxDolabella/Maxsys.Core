using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Maxsys.ModelCore.Interfaces.Repositories;

/// <summary>
/// Provides an interface to access data from a repository of <typeparamref name="TEntity"/>
/// </summary>
/// <typeparam name="TEntity">Type of entity used. Must be a <see langword="class"/>.</typeparam>
/// <typeparam name="TKey">Is the type of the key</typeparam>
public interface IRepositoryBase<TEntity, TKey> : IDisposable
    where TEntity : class
    where TKey : notnull
{
    /// <summary>
    /// A unique identifier for the Repository being used.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// A unique identifier for the Context being used.
    /// </summary>
    Guid ContextId { get; }

    #region CRUD

    /// <summary>
    /// Get all items from the repository that matches a condition asynchronously.
    /// </summary>
    /// <param name="predicate">Is the condition to select the items.</param>
    /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, specifies wether entity must be tracked.
    /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
    /// <para/>Default is <see langword="true"/>.</param>
    /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>an <see cref="IEnumerable{TEntity}"/> with all entities in repository.</returns>
    ValueTask<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true, CancellationToken token = default);

    /// <summary>
    /// Get all items from repository asynchronously.
    /// </summary>
    /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, specifies wether entity must be tracked.
    /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
    /// <para/>Default is <see langword="true"/>.</param>
    /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>an <see cref="IEnumerable{TEntity}"/> with all entities in repository.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(bool @readonly = true, CancellationToken token = default);

    /// <summary>
    /// Finds an entity with the given primary key value asynchronously.
    /// </summary>
    /// <param name="id">The value of the primary key for the entity to be found.</param>
    /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>The entity found, or <see langword="null"/>.</returns>
    ValueTask<TEntity?> FindAsync(TKey id, CancellationToken token = default);

    /// <summary>
    /// Adds  an <see cref="IEnumerable{T}"/> in the repository asynchronously, where T is <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="entities">Is the <see cref="IEnumerable{T}"/> of <typeparamref name="TEntity"/> to add.</param>
    /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is the <see cref="IEnumerable{T}"/> of <typeparamref name="TEntity"/> is added,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

    /// <summary>
    /// Add an object of type <typeparamref name="TEntity"/> in the repository asynchronously.
    /// </summary>
    /// <param name="obj">Is the <typeparamref name="TEntity"/> to add.</param>
    /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is added,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> AddAsync(TEntity obj, CancellationToken token = default);

    /// <summary>
    /// Update an object of type <typeparamref name="TEntity"/> in the repository asynchronously.
    /// </summary>
    /// <param name="obj">Is the <typeparamref name="TEntity"/> to update.</param>
    /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is updated,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> UpdateAsync(TEntity obj, CancellationToken token = default);

    /// <summary>
    /// Delete an object of type <typeparamref name="TEntity"/> from the repository asynchronously.
    /// </summary>
    /// <param name="key">Is the key of the <typeparamref name="TEntity"/> to remove.</param>
    /// <param name="token">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is deleted,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> RemoveAsync(TKey key, CancellationToken token = default);

    /// <summary>
    /// Asynchronously determines whether a sequence contains any elements that satisfy a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellation = default);

    /// <summary>
    /// Asynchronously returns the number of elements in a sequence that satisfy a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    ValueTask<int> CountAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellation = default);

    #endregion CRUD
}

/// <summary>
/// Provides an interface to access data from a repository.
/// The key is <see langword="object"/> by default.
/// </summary>
/// <typeparam name="TEntity">Type of entity used. Must be a <see langword="class"/>.</typeparam>
public interface IRepositoryBase<TEntity> : IRepositoryBase<TEntity, object>
    where TEntity : class
{ }