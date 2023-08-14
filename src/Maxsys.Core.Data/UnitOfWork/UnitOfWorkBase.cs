using FluentValidation;
using FluentValidation.Results;
using Maxsys.Core.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Maxsys.Core.Data;

/// <inheritdoc cref="IUnitOfWork"/>
public abstract class UnitOfWorkBase<TContext> : IUnitOfWork where TContext : DbContext
{
    protected readonly ILogger _logger;
    protected readonly TContext _context;
    protected int _semaphore = 0;
    private IDbContextTransaction? Transaction { get; set; }

    public Guid Id { get; } = Guid.NewGuid();
    public Guid ContextId { get; }

    protected UnitOfWorkBase(ILogger<UnitOfWorkBase<TContext>> logger, TContext context)
    {
        _logger = logger;
        _context = context;
        ContextId = context.ContextId.InstanceId;
    }

    /// <inheritdoc cref="IUnitOfWork"/>
    public virtual async ValueTask BeginTransactionAsync(string? name = null, CancellationToken token = default)
    {
        var transactionName = string.IsNullOrWhiteSpace(name) ? Guid.NewGuid().ToString() : name;

        if (_semaphore == 0)
        {
            _logger.LogInformation("Beginning a new transaction...");

            Transaction = /*_context.Database.CurrentTransaction ??*/ await _context.Database.BeginTransactionAsync(token);
        }

        _semaphore++;
        _logger.LogInformation("Transaction[{name}] | Semaphore[{semaphore}]", transactionName, _semaphore);
    }

    /// <inheritdoc cref="IUnitOfWork"/>
    public virtual async ValueTask CommitTransactionAsync(CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
            await RollbackTransactionAsync(CancellationToken.None);

        _semaphore--;

        _logger.LogInformation("Commiting Transaction | Semaphore[{semaphore}].", _semaphore);

        if (_semaphore == 0 && Transaction is not null)
        {
            _logger.LogInformation("Commiting Transaction");
            await Transaction.CommitAsync(token);
        }
    }

    /// <inheritdoc cref="IUnitOfWork"/>
    public virtual async ValueTask RollbackTransactionAsync(CancellationToken cancellation = default)
    {
        if (cancellation.IsCancellationRequested)
            return;

        _logger.LogInformation("Rolling back Transaction | Semaphore[{semaphore}].", _semaphore);

        if (Transaction is not null && _semaphore != 0)
        {
            _logger.LogWarning("Rolling back Transaction...");
            await Transaction.RollbackAsync(CancellationToken.None);
        }

        _semaphore = 0;
    }

    /// <inheritdoc cref="IUnitOfWork"/>
    public virtual async Task<ValidationResult> CommitAsync(CancellationToken cancellation = default)
    {
        var result = new ValidationResult();

        try
        {
            _ = await _context.SaveChangesAsync(cancellation);

            if (_semaphore == 0)
                _context.ChangeTracker.Clear();
        }
        catch (Exception ex)
        {
            RollBackDBContext();

            _logger.LogError(ex, CommonMessages.ERROR_SAVE);
            result.AddException(ex, CommonMessages.ERROR_SAVE);
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

    /// <summary/>
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
}