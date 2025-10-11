using FluentValidation;
using Maxsys.Core.Helpers;
using Maxsys.Core.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Maxsys.Core.Data;

/// <inheritdoc cref="IUnitOfWork"/>
public abstract class UnitOfWorkBase<TContext> : IUnitOfWork where TContext : DbContext
{
    private const string TRANSACTION_DEFAULT_NAME = "TRANSACTION";
    protected readonly ILogger _logger;
    protected readonly TContext _context;
    protected int _semaphore = 0;
    private IDbContextTransaction? Transaction { get; set; }

    public Guid Id { get; } = UIDGen.NewGuid(SequentialGuidOptions.None);
    public Guid ContextId { get; }

    protected UnitOfWorkBase(ILogger<UnitOfWorkBase<TContext>> logger, TContext context)
    {
        _logger = logger;
        _context = context;
        ContextId = context.ContextId.InstanceId;
    }

    public virtual async ValueTask BeginTransactionAsync(string? name = null, CancellationToken cancellationToken = default)
    {
        var normalizedName = string.IsNullOrWhiteSpace(name) ? TRANSACTION_DEFAULT_NAME : name.Trim();
        var transactionName = $"{normalizedName}-{Guid.NewGuid():N}";

        if (_semaphore == 0)
        {
            _logger.LogDebug("Beginning a new transaction[{tname}]...", transactionName);

            Transaction = /*_context.Database.CurrentTransaction ??*/ await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        _semaphore++;
        _logger.LogDebug("Transaction[{name}] | Semaphore[{semaphore}].", transactionName, _semaphore);
    }

    public virtual async ValueTask CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            await RollbackTransactionAsync(CancellationToken.None);

        _semaphore--;

        _logger.LogDebug("Commiting Transaction | Semaphore[{semaphore}].", _semaphore);

        if (_semaphore == 0 && Transaction is not null)
        {
            _logger.LogDebug("Commiting Transaction.");
            await Transaction.CommitAsync(cancellationToken);
        }
    }

    public virtual async ValueTask RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        _logger.LogDebug("Rolling back Transaction | Semaphore[{semaphore}].", _semaphore);

        if (Transaction is not null && _semaphore != 0)
        {
            _logger.LogWarning("Rolling back Transaction...");
            await Transaction.RollbackAsync(CancellationToken.None);
        }

        _semaphore = 0;
    }

    public virtual async Task<OperationResult> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Saving Changes...");

        var result = new OperationResult();

        try
        {
            var count = await _context.SaveChangesAsync(cancellationToken);

            _logger.LogDebug("<{count}> Changes Saved.\nSemaphore[{semaphore}]", count, _semaphore);

            if (_semaphore == 0)
            {
                ClearTracker();
            }


        }
        catch (Exception ex)
        {
            RollBackDBContext();

            _logger.LogError(ex, "{message}", GenericMessages.ERROR_SAVE);
            result.AddException(ex, GenericMessages.ERROR_SAVE);
        }

        return result;
    }

    private void RollBackDBContext()
    {
        _logger.LogWarning("Applying Rollback...");

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
    }

    #region DIPOSABLE IMPLEMENTATION

    protected bool _disposed = false;

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

    /// <inheritdoc cref="IDisposable"/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion DIPOSABLE IMPLEMENTATION

    public virtual void ClearTracker()
    {
        _logger.LogDebug("Cleaning Tracker...");
        _context.ChangeTracker.Clear();
    }
}