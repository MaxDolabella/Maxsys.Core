# Maxsys.Messaging

:mortar_board: Cada lançamento é um novo aprendizado!!

## 17.0.0
* :tada: Versão inicial do pacote. Substitui a mensageria baseada em MediatR que vivia dentro do `Maxsys.Core` (versões ≤ 16) — agora com **mediador próprio** (`MaxsysMediator`/`MaxsysBus`), sem dependências externas de messaging;
* :sparkles: Abstrações CQRS: `ICommand`/`ICommand<TResponse>`, `IQuery<TResponse>`, `IEvent` e seus handlers (`ICommandHandler`, `IQueryHandler`, `IEventHandler`), além das bases `CommandBase`, `QueryBase` e `QueryHandlerBase`;
* :sparkles: `IBus` como ponto único de entrada: `SendAsync` para commands/queries (handler único) e `Publish` para eventos (*broadcast* em paralelo para N handlers);
* :sparkles: Pipeline extensível via `IPipelineBehavior<,>`, com registro *open generic* através de `MessagingOptions.AddOpenBehavior`;
* :sparkles: `ValidationBehavior` incluído por padrão: validação automática via FluentValidation — quando o retorno herda de `OperationResult`, os erros viram `Notifications` (sem exception); caso contrário, lança `ValidationException`;
* :sparkles: Registro por *assembly scanning* via `AddMessaging<TEntry>()`, com detecção de handlers duplicados para commands/queries em tempo de startup;
* :sparkles: Suporte a `IBus` customizado via `AddMessaging<TEntry>(busFactory)` para integração com outras libs de messaging;
