# Maxsys.EventSourcing

Biblioteca de Event Sourcing e Domain Events no padrão DDD: tipos base para eventos de domínio (`DomainEvent`, `AuditableDomainEvent`), entidades com fila de eventos (`DomainEntity`) e persistência de eventos (`IEventStore`, `StoredEvent`) — integrada ao `IBus` de `Maxsys.Messaging` via `IEvent`.

## Abstrações

### IDomainEvent

Contrato para eventos de domínio DDD — algo que **aconteceu** no domínio: imutável, nomeado no passado. Estende `IEvent` de `Maxsys.Messaging`, então todo domain event pode ser publicado via `IBus.Publish` sem adaptadores.

+ `DateTime Timestamp`: momento UTC em que o evento ocorreu.
+ `string MessageType`: nome do tipo do evento — usado para persistência e roteamento.
+ Prefira herdar do *record* base `DomainEvent` em vez de implementar a interface diretamente.

```csharp
// Herdando do record base (recomendado) — Timestamp e MessageType são preenchidos automaticamente
public record OrderCreatedEvent(int OrderId, string CustomerId) : DomainEvent;
```

### IEventStore

Contrato de persistência de domain events. A implementação concreta (EF Core, MongoDB, arquivo...) fica no projeto consumidor.

+ `Task SaveAsync<T>(T @event) where T : class, IDomainEvent`

```csharp
public class SqlEventStore : IEventStore
{
    private readonly AppDbContext _context;
    public SqlEventStore(AppDbContext context) => _context = context;

    public async Task SaveAsync<T>(T @event) where T : class, IDomainEvent
    {
        _context.StoredEvents.Add(StoredEvent.Create(@event));
        await _context.SaveChangesAsync();
    }
}

// Registro no DI
services.AddScoped<IEventStore, SqlEventStore>();
```

## Eventos de Domínio

### DomainEvent

*Record* base abstrato para domain events. Preenche `Timestamp` (UTC) e `MessageType` (nome do tipo) automaticamente no construtor.

```csharp
// Simples
public record OrderCreatedEvent(int OrderId, string CustomerId, decimal Total) : DomainEvent;

// Com propriedade extra
public record UserDeactivatedEvent(int UserId, string Reason) : DomainEvent
{
    public string DeactivatedBy { get; init; } = string.Empty;
}
```

### AuditableDomainEvent&lt;TKey&gt;

Domain event com suporte a `AuditLog` (de `Maxsys.Core.Audit`). Use quando o evento precisa rastrear **quem** o causou. `TKey` é o tipo do Id da entidade que gerou o evento.

+ `TKey Id`: Id da entidade geradora (parâmetro posicional do record).
+ `AuditLog Audit`: informações de auditoria.
+ `SetAudit(AuditLog audit)`: define o `AuditLog` — chame antes de publicar.

```csharp
public record ProductPriceChangedEvent(int ProductId, decimal OldPrice, decimal NewPrice)
    : AuditableDomainEvent<int>(ProductId);

// No service, antes de publicar:
var evt = new ProductPriceChangedEvent(product.Id, oldPrice, newPrice);
evt.SetAudit(currentUserAudit);
await _bus.Publish(evt, ct);
```

## Entidades

### DomainEntity | DomainEntity&lt;TKey&gt;

Entidade de domínio (herda de `Entity` de `Maxsys.Core`) com fila de domain events. Os eventos são enfileirados durante as operações de negócio e publicados pelo service **após persistir** a entidade.

+ `IReadOnlyCollection<DomainEvent> DomainEvents`: eventos enfileirados — ignore na configuração do EF Core (`builder.Ignore(x => x.DomainEvents)`).
+ `AddDomainEvent(DomainEvent domainEvent)`: enfileira um evento.
+ `RemoveDomainEvent(DomainEvent domainEvent)`: remove um evento específico da fila.
+ `ClearDomainEvents()`: limpa a fila — chame após publicar via `IBus`.
+ `DomainEntity<TKey>`: variante com chave única tipada (`IKey<TKey>`). `TKey` deve ser escalar simples (`int`, `Guid`, `string`...); para chave composta, herde de `DomainEntity` (sem `TKey`) e configure via `builder.HasKey(...)`.

```csharp
public class Order : DomainEntity<int>
{
    public string CustomerId { get; private set; } = null!;

    public static Order Create(string customerId)
    {
        var order = new Order { CustomerId = customerId };
        order.AddDomainEvent(new OrderCreatedEvent(order.Id, customerId));
        return order;
    }

    public void Cancel(string reason)
    {
        AddDomainEvent(new OrderCancelledEvent(Id, reason));
    }
}
```

Fluxo completo no service — persistir, publicar, limpar:

```csharp
public async Task<OperationResult> CreateAsync(string customerId, CancellationToken ct)
{
    var order = Order.Create(customerId);

    await _repository.AddAsync(order, ct);
    await _unitOfWork.CommitAsync(ct);

    // Publica os eventos enfileirados somente após a persistência
    foreach (var domainEvent in order.DomainEvents)
        await _bus.Publish(domainEvent, ct);

    order.ClearDomainEvents();

    return new OperationResult();
}
```

Os handlers dos eventos usam `IEventHandler<TEvent>` de `Maxsys.Messaging`:

```csharp
public class StoreOrderCreatedEvent : IEventHandler<OrderCreatedEvent>
{
    private readonly IEventStore _eventStore;
    public StoreOrderCreatedEvent(IEventStore eventStore) => _eventStore = eventStore;

    public Task HandleAsync(OrderCreatedEvent @event, CancellationToken ct)
        => _eventStore.SaveAsync(@event);
}
```

## Persistência

### StoredEvent

Representação serializada de um domain event para persistência. Criado via *factory method* `Create<T>` — serializa o payload em JSON.

+ `Guid Id`: identificador único do evento armazenado.
+ `DateTime Timestamp`: momento UTC do armazenamento.
+ `string EventType`: nome do tipo do evento (equivale a `IDomainEvent.MessageType`).
+ `string Data`: payload do evento serializado como JSON.
+ `static StoredEvent Create<T>(T @event) where T : class, IDomainEvent`

```csharp
var evt = new OrderCreatedEvent(42, "customer-1", 199.90m);
var stored = StoredEvent.Create(evt);

Console.WriteLine(stored.EventType);  // "OrderCreatedEvent"
Console.WriteLine(stored.Data);       // JSON do evento
```
