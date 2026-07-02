# Maxsys.EventSourcing

:mortar_board: Cada lançamento é um novo aprendizado!!

## 17.0.0
* :tada: Versão inicial do pacote — funcionalidades de Domain Events/Event Sourcing extraídas do `Maxsys.Core` (versões ≤ 16) para um pacote dedicado;
* :sparkles: `IDomainEvent`: contrato para eventos de domínio DDD, estendendo `IEvent` de `Maxsys.Messaging` — domain events publicáveis direto via `IBus.Publish`;
* :sparkles: `DomainEvent`: *record* base abstrato que preenche `Timestamp` (UTC) e `MessageType` automaticamente;
* :sparkles: `AuditableDomainEvent<TKey>`: domain event com `AuditLog` para rastrear quem causou o evento;
* :sparkles: `DomainEntity`/`DomainEntity<TKey>`: entidades com fila de domain events (`AddDomainEvent`, `RemoveDomainEvent`, `ClearDomainEvents`);
* :sparkles: `IEventStore`: contrato de persistência de eventos — implementação concreta fica no projeto consumidor;
* :sparkles: `StoredEvent`: representação serializada (JSON) de um domain event, criada via `StoredEvent.Create<T>`;
