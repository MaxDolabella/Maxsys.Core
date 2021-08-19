using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Maxsys.ModelCore.Interfaces.Repositories
{
    /// <summary>
    /// Provides an interface to access data from a repository of <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity">Type of entity used. Must be a <see langword="class"/>.</typeparam>
    /// <typeparam name="TKey">Is the type of the key</typeparam>
    public interface IRepositoryBase<TEntity, TKey> : IDisposable
        where TEntity : class
    {
        #region Synchronous

        /// <summary>
        /// Get all items from repository.
        /// </summary>
        /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
        /// Entity Framework</see>, specifies wether entity must be tracked.
        /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
        /// <para/>Default is <see langword="true"/>.</param>
        /// <returns>an <see cref="IEnumerable{TEntity}"/> with all entities in repository.</returns>
        IEnumerable<TEntity> GetAll(bool @readonly = true);

        /// <summary>
        /// Get all items from the repository that match a condition.
        /// </summary>
        /// <param name="predicate">Is the condition to select the items.</param>
        /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
        /// Entity Framework</see>, specifies wether entity must be tracked.
        /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
        /// <para/>Default is <see langword="true"/>.</param>
        /// <returns>an <see cref="IEnumerable{TEntity}"/> with all entities in repository.</returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool @readonly = true);

        /// <summary>
        /// Get an object of type <typeparamref name="TEntity"/> from the repository given a key.
        /// </summary>
        /// <param name="key">Is the key of the item in repository.</param>
        /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
        /// Entity Framework</see>, specifies wether entity must be tracked.
        /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
        /// <para/>Default is <see langword="true"/>.</param>
        /// <returns>An entity of type <typeparamref name="TEntity"/>.</returns>
        TEntity GetById(TKey key, bool @readonly = true);

        /// <summary>
        /// Add an object of type <typeparamref name="TEntity"/> in the repository.
        /// </summary>
        /// <param name="obj">Is the <typeparamref name="TEntity"/> to add.</param>
        /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is added,
        /// otherwise, <see langword="false"/></returns>
        bool Add(TEntity obj);

        /// <summary>
        /// Update an object of type <typeparamref name="TEntity"/> in the repository.
        /// </summary>
        /// <param name="obj">Is the <typeparamref name="TEntity"/> to update.</param>
        /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is updated,
        /// otherwise, <see langword="false"/></returns>
        bool Update(TEntity obj);

        /// <summary>
        /// Delete an object of type <typeparamref name="TEntity"/> from the repository.
        /// </summary>
        /// <param name="obj">Is the <typeparamref name="TEntity"/> to remove.</param>
        /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is deleted,
        /// otherwise, <see langword="false"/></returns>
        bool Remove(TEntity obj);

        /// <summary>
        /// Delete an object of type <typeparamref name="TEntity"/> from the repository.
        /// </summary>
        /// <param name="key">Is the key of the <typeparamref name="TEntity"/> to remove.</param>
        /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is deleted,
        /// otherwise, <see langword="false"/></returns>
        bool Remove(TKey key);

        //void AddRange(params TEntity[] objs);

        //void RemoveRange(params TEntity[] objs);

        #endregion Synchronous

        #region Asynchronous

        /// <summary>
        /// Get all items from repository asynchronously.
        /// </summary>
        /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
        /// Entity Framework</see>, specifies wether entity must be tracked.
        /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
        /// <para/>Default is <see langword="true"/>.</param>
        /// <returns>an <see cref="IEnumerable{TEntity}"/> with all entities in repository.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(bool @readonly = true);

        /// <summary>
        /// Get all items from the repository that match a condition asynchronously.
        /// </summary>
        /// <param name="predicate">Is the condition to select the items.</param>
        /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
        /// Entity Framework</see>, specifies wether entity must be tracked.
        /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
        /// <para/>Default is <see langword="true"/>.</param>
        /// <returns>an <see cref="IEnumerable{TEntity}"/> with all entities in repository.</returns>
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true);

        /// <summary>
        /// Get an object of type <typeparamref name="TEntity"/> from the repository given a key asynchronously.
        /// </summary>
        /// <param name="key">Is the key of the item in repository.</param>
        /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
        /// Entity Framework</see>, specifies wether entity must be tracked.
        /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
        /// <para/>Default is <see langword="true"/>.</param>
        /// <returns>An entity of type <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> GetByIdAsync(TKey key, bool @readonly = true);

        /// <summary>
        /// Add an object of type <typeparamref name="TEntity"/> in the repository asynchronously.
        /// </summary>
        /// <param name="obj">Is the <typeparamref name="TEntity"/> to add.</param>
        /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is added,
        /// otherwise, <see langword="false"/></returns>
        Task<bool> AddAsync(TEntity obj);

        /// <summary>
        /// Update an object of type <typeparamref name="TEntity"/> in the repository asynchronously.
        /// </summary>
        /// <param name="obj">Is the <typeparamref name="TEntity"/> to update.</param>
        /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is updated,
        /// otherwise, <see langword="false"/></returns>
        Task<bool> UpdateAsync(TEntity obj);

        /// <summary>
        /// Delete an object of type <typeparamref name="TEntity"/> from the repository asynchronously.
        /// </summary>
        /// <param name="obj">Is the <typeparamref name="TEntity"/> to remove.</param>
        /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is deleted,
        /// otherwise, <see langword="false"/></returns>
        Task<bool> RemoveAsync(TEntity obj);

        /// <summary>
        /// Delete an object of type <typeparamref name="TEntity"/> from the repository asynchronously.
        /// </summary>
        /// <param name="key">Is the key of the <typeparamref name="TEntity"/> to remove.</param>
        /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is deleted,
        /// otherwise, <see langword="false"/></returns>
        Task<bool> RemoveAsync(TKey key);

        #endregion Asynchronous

        // Alternative for EFCore:
        /*
        IEnumerable<TEntity> GetAll(bool @readonly
            , Func<IQueryable<TEntity>
            , IIncludableQueryable<TEntity, object>> include = null);
        */
    }

    /// <summary>
    /// Provides an interface to access data from a repository.
    /// The key is <see langword="object"/> by default.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity used. Must be a <see langword="class"/>.</typeparam>
    public interface IRepositoryBase<TEntity> : IRepositoryBase<TEntity, object>
        where TEntity : class
    { }
}