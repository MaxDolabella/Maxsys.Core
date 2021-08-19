using FluentValidation.Results;
using System;
using System.Threading.Tasks;

namespace Maxsys.DataCore.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();

        ValidationResult SaveChanges();

        void RollBack();

        Task<ValidationResult> SaveChangesAsync();

        Task RollbackAsync();
    }
}