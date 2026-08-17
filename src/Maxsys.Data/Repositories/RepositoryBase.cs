using Maxsys.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.Data;

/// <inheritdoc cref="IRepository"/>
public abstract class RepositoryBase : IRepository, IDisposable
{
    #region FIELDS

    protected readonly DbContext Context;
    protected bool _disposed = false;

    #endregion FIELDS

    #region CONSTRUCTOR

    public RepositoryBase(DbContext context)
    {
        Context = context;
        ContextId = context.ContextId.InstanceId;
    }

    #endregion CONSTRUCTOR

    #region PROPERTIES

    public Guid Id { get; } = Guid.NewGuid();
    public Guid ContextId { get; }

    #endregion PROPERTIES

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Allows derived classes to override disposal behaviour. Repository does not dispose the shared DbContext by default.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Intentionally not disposing the DbContext here. Context lifetime is usually managed by a UnitOfWork or DI container.
        }

        _disposed = true;
    }
}