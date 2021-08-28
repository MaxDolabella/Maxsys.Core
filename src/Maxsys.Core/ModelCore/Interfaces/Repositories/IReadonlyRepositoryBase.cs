using System;

namespace Maxsys.ModelCore.Interfaces.Repositories
{
    /// <summary>
    /// Provides an interface to readonly access data from a repository of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity used. Must be a <see langword="class"/>.</typeparam>
    public interface IReadonlyRepositoryBase<TEntity>
        : IDisposable where TEntity : class
    { }
}