using FluentValidation.Results;

namespace Maxsys.Core.Interfaces.Data;

public interface IUnitOfWork : IDisposable
{
    Guid Id { get; }
    Guid ContextId { get; }

    ValueTask BeginTransactionAsync(string? name = null, CancellationToken cancellation = default);

    ValueTask CommitTransactionAsync(CancellationToken cancellation = default);

    /// <summary>
    /// Faz o rollback
    /// </summary>
    ValueTask RollbackTransactionAsync(CancellationToken cancellation = default);

    /// <summary>
    /// Executa o SaveChanges().
    /// <para/>
    /// ATENÇÃO: Uma vez usando o EF Core, também limpa o ChangeTracker.
    /// </summary>
    /// <returns></returns>
    Task<ValidationResult> CommitAsync(CancellationToken cancellation = default);
}