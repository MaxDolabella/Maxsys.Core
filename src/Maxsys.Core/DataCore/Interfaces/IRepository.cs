using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Maxsys.ModelCore.Filtering;
using Maxsys.ModelCore.Listing;
using Maxsys.ModelCore.Sorting;

namespace Maxsys.DataCore.Interfaces;

/// <summary>
/// Provides an interface to access data.
/// </summary>
public interface IRepository : IDisposable
{
    /// <summary>
    /// A unique identifier for the Repository being used.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// A unique identifier for the Context being used.
    /// </summary>
    Guid ContextId { get; }
}

/// <summary>
/// Provides an interface to access data from a repository of <typeparamref name="TEntity"/>
/// </summary>
/// <typeparam name="TEntity">Type of entity used. Must be a <see langword="class"/>.</typeparam>
/// <typeparam name="TKey">Is the type of the key</typeparam>
public interface IRepository<TEntity, TKey> : IRepository
    where TEntity : class
    where TKey : notnull
{
    #region CRUD

    /// <summary>
    /// Get the first item from the repository that matches a condition asynchronously.
    /// </summary>
    /// <param name="predicate">Is the condition to select the items.</param>
    /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, specifies wether entity must be tracked.
    /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
    /// <para/>Default is <see langword="true"/>.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>an <typeparamref name="TEntity"/> from the repository.</returns>
    ValueTask<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true, CancellationToken cancellation = default);

    /// <summary>
    /// Get all items from the repository that matches a condition asynchronously.
    /// </summary>
    /// <param name="predicate">Is the condition to select the items.</param>
    /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, specifies wether entity must be tracked.
    /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
    /// <para/>Default is <see langword="true"/>.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>an <see cref="IEnumerable{TEntity}"/> with all entities in repository.</returns>
    ValueTask<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true, CancellationToken cancellation = default);

    /// <summary>
    /// Get all items from repository asynchronously.
    /// </summary>
    /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, specifies wether entity must be tracked.
    /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
    /// <para/>Default is <see langword="true"/>.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>an <see cref="IEnumerable{TEntity}"/> with all entities in repository.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(bool @readonly = true, CancellationToken cancellation = default);

    /// <summary>
    /// Add an object of type <typeparamref name="TEntity"/> in the repository asynchronously.
    /// </summary>
    /// <param name="entity">Is the <typeparamref name="TEntity"/> to add.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is added,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> AddAsync(TEntity entity, CancellationToken cancellation = default);

    /// <summary>
    /// Adds  an <see cref="IEnumerable{T}"/> in the repository asynchronously, where T is <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="entities">Is the <see cref="IEnumerable{T}"/> of <typeparamref name="TEntity"/> to add.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is the <see cref="IEnumerable{T}"/> of <typeparamref name="TEntity"/> is added,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default);

    /// <summary>
    /// Update an object of type <typeparamref name="TEntity"/> in the repository asynchronously.
    /// </summary>
    /// <param name="entity">Is the <typeparamref name="TEntity"/> to update.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is updated,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> UpdateAsync(TEntity entity, CancellationToken cancellation = default);

    /// <summary>
    /// Update an object of type <typeparamref name="TEntity"/> in the repository asynchronously.
    /// </summary>
    /// <param name="entities">Is the <see cref="IEnumerable{T}"/> of <typeparamref name="TEntity"/> to update.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is updated,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default);

    /// <summary>
    /// Delete an object of type <typeparamref name="TEntity"/> from the repository asynchronously.
    /// </summary>
    /// <param name="key">Is the key of the <typeparamref name="TEntity"/> to remove.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is deleted,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> DeleteAsync(TKey key, CancellationToken cancellation = default);

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

    /// <summary>
    /// Gets an IQueryable given a predicate.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="readonly"></param>
    /// <returns><see cref="IEnumerable{TEntity}"/></returns>
    IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate, bool @readonly = true);

    #endregion CRUD
}

/// <summary>
/// Provides an interface to access data from a repository.
/// The key is <see langword="object"/> by default.
/// </summary>
/// <typeparam name="TEntity">Type of entity used. Must be a <see langword="class"/>.</typeparam>
public interface IRepository<TEntity> : IRepository<TEntity, object>
    where TEntity : class
{ }

/// <summary>
/// Provides an interface to access data from a repository of <typeparamref name="TEntity"/>
/// with filtering using a <typeparamref name="TFilter"/> filter.
/// </summary>
/// <typeparam name="TEntity">Type of entity used. Must be a <see langword="class"/>.</typeparam>
/// <typeparam name="TFilter">Is the type of the key</typeparam>
/// <typeparam name="TKey">Is the type of the key</typeparam>
public interface IRepository<TEntity, TFilter, TKey> : IRepository<TEntity, TKey>
    where TEntity : class
    where TFilter : IFilter<TEntity>
    where TKey : notnull
{
    /// <summary>
    /// Asynchronously returns the number of elements in a sequence that satisfy a filter.
    /// </summary>
    /// <param name="filters">A filter to test each element for a condition.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    ValueTask<int> CountAsync(TFilter filters, CancellationToken cancellation = default);

    /// <summary>
    /// Asynchronously determines whether a sequence contains any elements that satisfy a filter.
    /// </summary>
    /// <param name="filters">A filter to test each element for a condition.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    ValueTask<bool> AnyAsync(TFilter filters, CancellationToken cancellation = default);

    /// <summary>
    /// Asynchronously get all items from the repository that matches a filter.
    /// </summary>
    /// <param name="filters">A filter to test each element for a condition.</param>
    /// <param name="criteria">criteria to retrieve the data (pagination and sort).</param>
    /// <param name="sortColumnSelector">the implementaion of sort column selector.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>an <see cref="IReadOnlyList{TDestination}"/> from the repository.</returns>
    Task<IReadOnlyList<TDestination>> GetAsync<TDestination>(TFilter filters, Criteria criteria, ISortColumnSelector<TDestination> sortColumnSelector, CancellationToken cancellation = default) where TDestination : class;

    /// <summary>
    /// Get the first item from the repository that matches a condition asynchronously.
    /// </summary>
    /// <param name="predicate">Is the condition to select the items.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <typeparam name="TDestination">is the destination type. Needs mapping.</typeparam>
    /// <returns>an <typeparamref name="TDestination"/> from the repository.</returns>
    Task<TDestination?> GetFirstAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default) where TDestination : class;
}