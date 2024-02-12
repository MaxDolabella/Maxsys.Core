using FluentValidation.Results;

namespace Maxsys.Core.Interfaces.Data;

public interface IUnitOfWork : IDisposable
{
    Guid Id { get; }
    Guid ContextId { get; }

    ValueTask BeginTransactionAsync(string? name = null, CancellationToken cancellationToken = default);

    ValueTask CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Faz o rollback
    /// </summary>
    ValueTask RollbackTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executa o SaveChanges().
    /// <para/>
    /// ATENÇÃO: Uma vez usando o EF Core, também limpa o ChangeTracker.
    /// </summary>
    Task<OperationResult> SaveChangesAsync(CancellationToken cancellationToken = default);
}