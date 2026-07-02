<div align="center">
<img src="logo.png" alt="drawing" width="128" />
<h1>Maxsys Messaging</h1>
</div>

[![License](https://img.shields.io/github/license/maxdolabella/maxsys.core)](LICENSE)

**Maxsys.Messaging** é uma biblioteca de mensageria **CQRS** para aplicações Maxsys, com **mediador próprio** — sem dependências externas (não usa MediatR).

Fornece os contratos `ICommand`, `ICommand<TResponse>`, `IQuery<TResponse>` e `IEvent`, seus handlers (`ICommandHandler`, `IQueryHandler`, `IEventHandler`), o ponto de entrada `IBus` e um pipeline de comportamentos (`IPipelineBehavior`) com `ValidationBehavior` (FluentValidation) já incluído.

## :package: Nuget
![Nuget](https://img.shields.io/nuget/v/Maxsys.Messaging)

```xml
    <PackageReference Include="Maxsys.Messaging" Version="17.0.0" />
```

## :gear: Uso

```csharp
// Registro
services.AddMessaging<IApplicationEntry>();

// Envio
var result = await bus.SendAsync(new CriarClienteCommand(...));
```

## :link: Dependências

- `Maxsys.Core`

## :dart: Target
`.NET 10`
