using FluentValidation.Results;
using Maxsys.DataCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.AppCore
{
    /// <summary>
    /// Base class for application service classes
    /// <para/>
    /// Implements <see cref="IDisposable"/>
    /// </summary>
    public abstract class ApplicationServiceBase : IDisposable
    {
        private readonly IUnitOfWork _uow;
        protected ICollection<IDisposable> DisposableComponents { get; } = new HashSet<IDisposable>();

        protected ApplicationServiceBase(IUnitOfWork uow) => _uow = uow;

        protected void BeginTransaction() => _uow.BeginTransaction();

        protected ValidationResult Commit() => _uow.SaveChanges();

        protected void Rollback() => _uow.RollBack();

        protected async Task<ValidationResult> CommitAsync() => await _uow.SaveChangesAsync();

        protected async Task RollbackAsync() => await _uow.RollbackAsync();

        #region DIPOSABLE IMPLEMENTATION

        protected bool _disposed = false;

        /// <summary>
        /// Disposes components added in <see cref="DisposableComponents"/> property.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                foreach (var disposable in DisposableComponents)
                    disposable?.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion DIPOSABLE IMPLEMENTATION
    }
}