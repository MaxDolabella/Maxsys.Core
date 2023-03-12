using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Maxsys.DataCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Maxsys.Core.Data;

/// <summary>
/// Provides an abstract object to wraps a database transaction.
/// This class implements <see cref="IUnitOfWork"/> and <see cref="System.IDisposable"/>
/// to allow use inside using blocks, and
/// should automatically close the database transaction (without saving) when disposed.
/// </summary>
public abstract class UnitOfWorkBase<TContext> : IUnitOfWork
    where TContext : DbContext
{
    /// <inheritdoc/>
    public Guid Id { get; } = Guid.NewGuid();

    /// <inheritdoc/>
    public Guid ContextId { get; }

    private readonly ILogger _logger;
    private readonly TContext _context;
    private int _semaphore = 0;
    private IDbContextTransaction? Transaction { get; set; }

    /// <summary>
    /// Constructor for EFUnitOfWork
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="context"></param>
    protected UnitOfWorkBase(ILogger<UnitOfWorkBase<TContext>> logger, TContext context)
    {
        _logger = logger;
        _context = context;

        ContextId = context.ContextId.InstanceId;
    }

    /// <inheritdoc/>
    public async ValueTask BeginTransactionAsync(CancellationToken cancellation = default)
    {
        if (_semaphore == 0)
        {
            _logger.LogInformation("Beginning a new transaction.");
            Transaction = /*_context.Database.CurrentTransaction ??*/ await _context.Database.BeginTransactionAsync(cancellation);
        }

        _semaphore++;
        _logger.LogInformation("Transaction Semaphore={semaphore}.", _semaphore);
    }

    /// <inheritdoc/>
    public async ValueTask CommitTransactionAsync(CancellationToken cancellation = default)
    {
        _semaphore--;
        _logger.LogInformation("Transaction Semaphore={semaphore}.", _semaphore);

        if (cancellation.IsCancellationRequested)
            await RollbackTransactionAsync(CancellationToken.None);
        else if (_semaphore == 0 && Transaction is not null)
        {
            _logger.LogInformation("Commiting Transaction.");
            await Transaction.CommitAsync(cancellation);
        }
    }

    /// <inheritdoc/>
    public async ValueTask RollbackTransactionAsync(CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
            return;

        _semaphore = 0;

        if (Transaction is not null)
        {
            _logger.LogWarning("Applying Transaction Rollback.");
            await Transaction.RollbackAsync(CancellationToken.None);
        }
    }

    /// <inheritdoc/>
    public async Task<ValidationResult> CommitAsync(CancellationToken cancellation = default)
    {
        var result = new ValidationResult();

        try
        {
            if (cancellation.IsCancellationRequested)
                return result.AddError("Cancelled operation");

            _ = await _context.SaveChangesAsync(cancellation);

            _context.ChangeTracker.Clear();
        }
        catch (Exception ex)
        {
            await RollbackAsync(CancellationToken.None);

            _logger.LogError(ex, "Error at commit.");
            result.AddException(ex, "Error at commit.");
        }

        return result;
    }

    /// <inheritdoc/>
    public ValueTask RollbackAsync(CancellationToken cancellation = default)
    {
        _logger.LogWarning("Applying Rollback.");

        var changedEntries = _context.ChangeTracker.Entries()
            .Where(x => x.State != EntityState.Unchanged);

        foreach (var entry in changedEntries)
        {
            if (entry.State == EntityState.Modified)
                entry.CurrentValues.SetValues(entry.OriginalValues);

            entry.State = entry.State switch
            {
                EntityState.Added => EntityState.Detached,
                EntityState.Modified or EntityState.Deleted => EntityState.Unchanged,
                _ => entry.State
            };
        }

        _logger.LogWarning("Rollback applied");

        return ValueTask.CompletedTask;
    }

    #region DIPOSABLE IMPLEMENTATION

    /// <summary/>
    protected bool _disposed = false;

    /// <summary/>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Database.CurrentTransaction?.Dispose();
            }
        }
        _disposed = true;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion DIPOSABLE IMPLEMENTATION
}