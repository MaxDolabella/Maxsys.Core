using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Maxsys.Core.Data.Interfaces;

/// <summary>
/// Provides an interface to wraps a database transaction.
/// <br/>
/// This class implements <see cref="IDisposable"/> to allow use inside <c>using</c> blocks, and should automatically
/// close the database transaction (without saving) when disposed.
/// </summary>
/// <remarks>Basic Unit of Work pattern as described by Martin Fowler in this <see href="http://martinfowler.com/eaaCatalog/unitOfWork.html">link</see>.</remarks>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// A unique identifier for the UnitOfWork being used.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// A unique identifier for the Context being used.
    /// </summary>
    Guid ContextId { get; }

    /// <summary>
    /// Asynchronously starts a new transaction.
    /// </summary>
    /// <param name="cancellation">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    ValueTask BeginTransactionAsync(CancellationToken cancellation = default);

    /// <summary>
    /// Asynchronously commits a transaction.
    /// </summary>
    /// <param name="cancellation">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    ValueTask CommitTransactionAsync(CancellationToken cancellation = default);

    /// <summary>
    /// Asynchronously rollback a transaction.
    /// </summary>
    /// <param name="cancellation">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    ValueTask RollbackTransactionAsync(CancellationToken cancellation = default);

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <param name="token">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>

    Task<ValidationResult> CommitAsync(CancellationToken token = default);
}