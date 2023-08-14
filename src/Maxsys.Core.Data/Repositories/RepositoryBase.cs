using Maxsys.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.Core.Data;

/// <inheritdoc cref="IRepository"/>
public abstract class RepositoryBase : IRepository, IDisposable
{
    #region FIELDS

    protected readonly DbContext Context;

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
        GC.SuppressFinalize(this);
    }
}