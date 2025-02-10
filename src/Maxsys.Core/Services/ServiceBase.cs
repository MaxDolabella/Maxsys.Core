using Maxsys.Core.Interfaces.Services;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IService"/>
public abstract class ServiceBase : IService
{
    /// <inheritdoc />
    public Guid Id { get; } = Guid.NewGuid();

    #region Dispose Pattern

    protected bool _disposed = false;

    /// <summary>
    /// Dispose Pattern.<br/>
    /// Veja mais em: <see href="https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose"/>
    /// </summary>
    public void Dispose()
    {
        // Dispose of unmanaged resources.
        Dispose(true);
        // Suppress finalization.
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose Pattern.<br/>
    /// Veja mais em: <see href="https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose"/>
    /// </summary>
    /// <remarks>
    /// <code>
    /// if (_disposed)
    /// {
    ///     return;
    /// }
    ///
    /// if (disposing)
    /// {
    ///     // dispose managed state (managed objects).
    /// }
    ///
    /// // free unmanaged resources (unmanaged objects) and override a finalizer below.
    /// // set large fields to null.
    ///
    /// _disposed = true;
    /// </code>
    /// </remarks>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            // NOTE: dispose managed state (managed objects).
        }

        // NOTE: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // NOTE: set large fields to null.

        _disposed = true;
    }

    #endregion Dispose Pattern
}