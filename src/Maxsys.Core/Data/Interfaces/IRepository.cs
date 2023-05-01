using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Maxsys.Core.Filtering;
using Maxsys.Core.Listing;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Data.Interfaces;

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
public interface IRepository<TKey, TEntity> : IRepository
    where TKey : notnull
    where TEntity : Entity<TKey>
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

    #endregion CRUD
}

/// <summary>
/// Provides an interface to access data from a repository of <typeparamref name="TEntity"/>
/// with filtering using a <typeparamref name="TFilter"/> filter.
/// </summary>
/// <typeparam name="TEntity">Type of entity used. Must be a <see langword="class"/>.</typeparam>
/// <typeparam name="TFilter">Is the type of the key</typeparam>
/// <typeparam name="TKey">Is the type of the key</typeparam>
public interface IRepository<TKey, TEntity, TFilter> : IRepository<TKey, TEntity>
    where TKey : notnull
    where TEntity : Entity<TKey>
    where TFilter : IFilter<TEntity>
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
    /// <remarks>Mapping required: <typeparamref name="TEntity"/> ➔ <typeparamref name="TDestination"/>.</remarks>
    Task<IReadOnlyList<TDestination>> GetMappedAsync<TDestination>(TFilter filters, Criteria criteria, ISortColumnSelector<TDestination> sortColumnSelector, CancellationToken cancellation = default) where TDestination : class;

    /// <summary>
    /// Get the first item from the repository that matches a condition asynchronously.
    /// </summary>
    /// <param name="filters">A filter to test each element for a condition.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <typeparam name="TDestination">is the destination type. Needs mapping.</typeparam>
    /// <returns>an <typeparamref name="TDestination"/> from the repository.</returns>
    /// <remarks>Mapping required: <typeparamref name="TEntity"/> ➔ <typeparamref name="TDestination"/>.</remarks>
    Task<TDestination?> GetFirstMappedAsync<TDestination>(TFilter filters, CancellationToken cancellation = default) where TDestination : class;

    /// <summary>
    /// Get the first item from the repository that matches a condition asynchronously.
    /// </summary>
    /// <param name="predicate">Is the condition to select the items.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <typeparam name="TDestination">is the destination type. Needs mapping.</typeparam>
    /// <returns>an <typeparamref name="TDestination"/> from the repository.</returns>
    /// <remarks>Mapping required: <typeparamref name="TEntity"/> ➔ <typeparamref name="TDestination"/>.</remarks>
    Task<TDestination?> GetFirstMappedAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default) where TDestination : class;
}