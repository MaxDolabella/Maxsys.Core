using Maxsys.Core.Interfaces.Services;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IService"/>
public abstract class ServiceBase : IService
{
    /// <inheritdoc />
    public Guid Id { get; } = Guid.NewGuid();

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}