using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.ModelCore.Interfaces.Services
{
    /// <summary>
    /// Provides an interface for basic entity crud operations.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IServiceBase<TEntity> : IDisposable where TEntity : class
    {
        #region Synchronous

        /// <summary>
        /// Get an object of type <typeparamref name="TEntity"/>
        /// given an id and with no dependencies.
        /// </summary>
        /// <param name="id">Is the id of the item.</param>
        /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
        /// Entity Framework</see>, specifies wether entity must be tracked.
        /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
        /// <para/>Default is <see langword="true"/>.</param>
        /// <returns>An entity of type <typeparamref name="TEntity"/>.</returns>
        TEntity GetById(object id, bool @readonly = true);

        /// <summary>
        /// Get all items.
        /// </summary>
        /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
        /// Entity Framework</see>, specifies wether entity must be tracked.
        /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
        /// <para/>Default is <see langword="true"/>.</param>
        /// <returns>an <see cref="IEnumerable{TEntity}"/> with all entities in repository.</returns>
        IEnumerable<TEntity> GetAll(bool @readonly = true);

        /// <summary>
        /// Add an object of type <typeparamref name="TEntity"/>
        /// and returns the validation from the operation.
        /// </summary>
        /// <param name="entity">Is the <typeparamref name="TEntity"/> to add.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the operation was successful or not.
        /// If not, <see cref="ValidationResult"/> will contain the operations errors.</returns>
        ValidationResult Add(TEntity entity);

        /// <summary>
        /// Update an object of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="entity">Is the <typeparamref name="TEntity"/> to update.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the operation was successful or not.
        /// If not, <see cref="ValidationResult"/> will contain the operations errors.</returns>
        ValidationResult Update(TEntity entity);

        /// <summary>
        /// Delete an object of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="id">Is the <typeparamref name="TEntity"/> to remove.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the operation was successful or not.
        /// If not, <see cref="ValidationResult"/> will contain the operations errors.</returns>
        ValidationResult Remove(object id);

        #endregion Synchronous

        #region Asynchronous

        /// <summary>
        /// Get an object of type <typeparamref name="TEntity"/>
        /// given an id and with no dependencies asynchronously.
        /// </summary>
        /// <param name="id">Is the id of the item.</param>
        /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
        /// Entity Framework</see>, specifies wether entity must be tracked.
        /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
        /// <para/>Default is <see langword="true"/>.</param>
        /// <returns>An entity of type <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> GetByIdAsync(object id, bool @readonly = true);

        /// <summary>
        /// Get all items asynchronously.
        /// </summary>
        /// <param name="readonly">For some ORMs like <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
        /// Entity Framework</see>, specifies wether entity must be tracked.
        /// If <paramref name="readonly"/> is <see langword="true"/>, must be tracked, otherwise, must not.
        /// <para/>Default is <see langword="true"/>.</param>
        /// <returns>an <see cref="IEnumerable{TEntity}"/> with all entities in repository.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(bool @readonly = true);

        /// <summary>
        /// Add an object of type <typeparamref name="TEntity"/> asynchronously
        /// and returns the validation from the operation.
        /// </summary>
        /// <param name="entity">Is the <typeparamref name="TEntity"/> to add.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the operation was successful or not.
        /// If not, <see cref="ValidationResult"/> will contain the operations errors.</returns>
        Task<ValidationResult> AddAsync(TEntity entity);

        /// <summary>
        /// Update an object of type <typeparamref name="TEntity"/> asynchronously.
        /// </summary>
        /// <param name="entity">Is the <typeparamref name="TEntity"/> to update.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the operation was successful or not.
        /// If not, <see cref="ValidationResult"/> will contain the operations errors.</returns>
        Task<ValidationResult> UpdateAsync(TEntity entity);

        /// <summary>
        /// Delete an object of type <typeparamref name="TEntity"/> asynchronously.
        /// </summary>
        /// <param name="id">Is the <typeparamref name="TEntity"/> to remove.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the operation was successful or not.
        /// If not, <see cref="ValidationResult"/> will contain the operations errors.</returns>
        Task<ValidationResult> RemoveAsync(object id);

        #endregion Asynchronous
    }
}