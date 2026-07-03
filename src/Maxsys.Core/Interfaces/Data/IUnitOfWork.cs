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
    /// Persiste as alterações pendentes.
    /// <para/>
    /// Implementações podem executar limpeza pós-persistência (ex.: em EF Core,
    /// <c>UnitOfWorkBase</c> limpa o ChangeTracker — comportamento configurável na implementação).
    /// </summary>
    Task<OperationResult> SaveChangesAsync(CancellationToken cancellationToken = default);
}