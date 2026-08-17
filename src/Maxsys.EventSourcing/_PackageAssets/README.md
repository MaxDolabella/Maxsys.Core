<div align="center">
<img src="logo.png" alt="drawing" width="128" />
<h1>Maxsys EventSourcing</h1>
</div>

[![License](https://img.shields.io/github/license/maxdolabella/maxsys.core)](LICENSE)

**Maxsys.EventSourcing** fornece a base para **Event Sourcing** e *Domain Events* (DDD) em aplicações Maxsys.

Inclui `IDomainEvent`, `DomainEvent` (record que preenche `Timestamp` e `MessageType` automaticamente), `AuditableDomainEvent<TKey>` (com trilha de auditoria), `DomainEntity` / `DomainEntity<TKey>` (entidade com fila interna de eventos), `StoredEvent` (evento serializado para persistência) e o contrato `IEventStore`.

## :package: Nuget
![Nuget](https://img.shields.io/nuget/v/Maxsys.EventSourcing)

```xml
    <PackageReference Include="Maxsys.EventSourcing" Version="17.0.0" />
```

## :gear: Uso

```csharp
public sealed record ClienteCriadoEvent(Guid Id) : DomainEvent;

// Na entidade
AddDomainEvent(new ClienteCriadoEvent(Id));
// Após persistir: publique via IBus e chame ClearDomainEvents().
```

## :link: Dependências

- `Maxsys.Core`
- `Maxsys.Messaging`

## :dart: Target
`.NET 10`
