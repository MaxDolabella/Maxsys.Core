namespace Maxsys.EventSourcing.Abstractions;

/// <summary>
/// Contrato para persistência de domain events.
/// Implemente para salvar eventos em banco de dados, arquivo ou qualquer outro meio.
/// </summary>
/// <remarks>
/// <example>
/// Implementação com EF Core:
/// <code>
/// public class SqlEventStore : IEventStore
/// {
///     private readonly AppDbContext _context;
///
///     public SqlEventStore(AppDbContext context) => _context = context;
///
///     public async Task SaveAsync&lt;T&gt;(T @event) where T : class, IDomainEvent
///     {
///         _context.StoredEvents.Add(StoredEvent.Create(@event));
///         await _context.SaveChangesAsync();
///     }
/// }
/// </code>
/// </example>
/// </remarks>
public interface IEventStore
{
    /// <summary>Persiste um domain event.</summary>
    Task SaveAsync<T>(T @event) where T : class, IDomainEvent;
}
