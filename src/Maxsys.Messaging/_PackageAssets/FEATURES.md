# Maxsys.Messaging

Biblioteca de mensageria CQRS para .NET 10 com mediador próprio (`MaxsysMediator`/`MaxsysBus`), sem dependências externas de messaging: Commands alteram estado, Queries leem dados e Events notificam múltiplos interessados — tudo despachado via `IBus`.

## Commands

### ICommand | ICommand&lt;TResponse&gt;

Interfaces *marker* para commands — operações que **alteram estado** (têm *side effects*).

+ `ICommand`: command sem retorno.
+ `ICommand<TResponse>`: command com retorno tipado (covariante).

```csharp
public class DeleteProductCommand : ICommand
{
    public int ProductId { get; set; }
}

public class CreateProductCommand : ICommand<OperationResult<int>>
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

### CommandBase | CommandBase&lt;TResponse&gt;

Classes base abstratas para commands. Uso opcional — servem como ancoragem de hierarquia quando se prefere herança a implementar a interface diretamente.

```csharp
public class CreateProductCommand : CommandBase<OperationResult<int>>
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

### ICommandHandler&lt;TCommand&gt; | ICommandHandler&lt;TCommand, TResponse&gt;

Handler de um command. Cada command deve ter **exatamente um** handler — handlers duplicados causam `InvalidOperationException` no registro.

+ `Task HandleAsync(TCommand command, CancellationToken ct)` — versão sem retorno.
+ `Task<TResponse> HandleAsync(TCommand command, CancellationToken ct)` — versão com retorno.

```csharp
public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, OperationResult<int>>
{
    private readonly IProductRepository _repo;
    public CreateProductCommandHandler(IProductRepository repo) => _repo = repo;

    public async Task<OperationResult<int>> HandleAsync(CreateProductCommand command, CancellationToken ct)
    {
        var product = new Product { Name = command.Name, Price = command.Price };
        await _repo.AddAsync(product, ct);

        return new OperationResult<int>(product.Id);
    }
}
```

## Queries

### IQuery&lt;TResponse&gt;

Interface *marker* para queries — **leitura pura**, sem *side effects*, sempre com retorno tipado.

```csharp
public record GetUserByIdQuery(int UserId) : IQuery<UserDTO?>;
```

### IQueryHandler&lt;TQuery, TResponse&gt;

Handler de uma query. Assim como commands, cada query deve ter exatamente um handler.

+ `Task<TResponse> HandleAsync(TQuery query, CancellationToken ct)`

```csharp
public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDTO?>
{
    private readonly IUserRepository _repo;
    public GetUserByIdQueryHandler(IUserRepository repo) => _repo = repo;

    public async Task<UserDTO?> HandleAsync(GetUserByIdQuery query, CancellationToken ct)
        => await _repo.GetByIdAsync<UserDTO>(query.UserId, ct);
}
```

### QueryBase&lt;TResponse&gt; | QueryHandlerBase&lt;TQuery, TResponse&gt;

Bases opcionais para queries e seus handlers.

+ `QueryBase<TResponse>`: *record* base — comparação por valor e imutabilidade natural.
+ `QueryHandlerBase<TQuery, TResponse>`: classe base — implemente o `HandleAsync` abstrato.

```csharp
public record GetUserByIdQuery(int UserId) : QueryBase<UserDTO?>;

public class GetUserByIdQueryHandler : QueryHandlerBase<GetUserByIdQuery, UserDTO?>
{
    private readonly IUserRepository _repo;
    public GetUserByIdQueryHandler(IUserRepository repo) => _repo = repo;

    public override async Task<UserDTO?> HandleAsync(GetUserByIdQuery query, CancellationToken ct)
        => await _repo.GetByIdAsync<UserDTO>(query.UserId, ct);
}
```

## Events

### IEvent

*Marker* base para eventos publicados via `IBus.Publish` — "algo aconteceu", *broadcast* para N handlers. Use diretamente para eventos de aplicação; para eventos de domínio DDD, use `IDomainEvent`/`DomainEvent` de `Maxsys.EventSourcing` (que estende `IEvent`).

```csharp
public record ProductCreatedEvent(int ProductId, string Name) : IEvent;
```

### IEventHandler&lt;TEvent&gt;

Handler de evento. Diferente de commands/queries, **múltiplos handlers** podem ser registrados para o mesmo evento — todos executados **em paralelo** (`Task.WhenAll`).

+ `Task HandleAsync(TEvent @event, CancellationToken ct)`

```csharp
public class SendEmailOnProductCreated : IEventHandler<ProductCreatedEvent>
{
    public Task HandleAsync(ProductCreatedEvent @event, CancellationToken ct)
        => _emailService.SendAsync($"Produto {@event.Name} criado.", ct);
}

public class InvalidateCacheOnProductCreated : IEventHandler<ProductCreatedEvent>
{
    public Task HandleAsync(ProductCreatedEvent @event, CancellationToken ct)
        => _cache.RemoveAsync("products", ct);
}
```

## Bus

### IBus

Ponto de entrada único para envio de mensagens. Injete `IBus` em controllers e services — o mediador localiza o handler certo automaticamente.

+ `Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken ct)` — command com retorno (handler único).
+ `Task SendAsync(ICommand command, CancellationToken ct)` — command sem retorno (handler único).
+ `Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken ct)` — query (handler único).
+ `Task Publish<TEvent>(TEvent @event, CancellationToken ct = default)` — evento (broadcast em paralelo para todos os handlers).

```csharp
[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly IBus _bus;
    public ProductsController(IBus bus) => _bus = bus;

    [HttpPost]
    public async Task<OperationResult<int>> Create(CreateProductCommand command, CancellationToken ct)
    {
        var result = await _bus.SendAsync(command, ct);

        if (result.IsValid)
            await _bus.Publish(new ProductCreatedEvent(result.Data, command.Name), ct);

        return result;
    }

    [HttpGet("{id}")]
    public async Task<UserDTO?> Get(int id, CancellationToken ct)
        => await _bus.SendAsync(new GetUserByIdQuery(id), ct);
}
```

## Pipeline

### IPipelineBehavior&lt;TRequest, TResponse&gt;

Intercepta a execução de um request (command com retorno ou query), permitindo lógica antes e depois do handler — logging, caching, transação, etc. Registre como *open generic* via `MessagingOptions.AddOpenBehavior` (ou direto no DI).

+ `Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken ct)`

```csharp
public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken ct)
    {
        _logger.LogInformation("Executando {Request}...", typeof(TRequest).Name);

        var response = await next();

        _logger.LogInformation("{Request} concluído.", typeof(TRequest).Name);

        return response;
    }
}
```

### ValidationBehavior&lt;TRequest, TResponse&gt;

Behavior de validação automática via FluentValidation, **incluído por padrão** no `AddMessaging`. Aplica-se a commands com retorno (`ICommand<TResponse>`): executa todos os `IValidator<TRequest>` registrados antes do handler.

+ Se `TResponse` herda de `OperationResult`: erros viram `Notification`s no resultado — **sem exception**.
+ Caso contrário: lança `ValidationException`.

```csharp
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Nome é obrigatório.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Preço deve ser positivo.");
    }
}

// Registre o validator no DI:
services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();

// Com CreateProductCommand : ICommand<OperationResult<int>>, um command inválido
// retorna OperationResult com Notifications — o handler nem chega a executar.
```

## Registro (DI)

### MessagingServiceCollectionExtensions (AddMessaging)

Métodos de extensão de `IServiceCollection` que registram o mediador Maxsys, o `IBus` padrão e **todos os handlers** por *assembly scanning*. Commands e queries com handler duplicado disparam `InvalidOperationException` no startup.

+ `AddMessaging<TEntry>(Action<MessagingOptions>? configure = null)` — varre o assembly de `TEntry`.
+ `AddMessaging(Action<MessagingOptions>? configure = null, params Assembly[] assemblies)` — varre os assemblies informados.
+ `AddMessaging<TEntry>(Func<IServiceProvider, IBus> busFactory, ...)` — substitui o `MaxsysBus` padrão por um `IBus` customizado (ex.: *wrapper* de outra lib de messaging).

```csharp
// Program.cs — TEntry é qualquer tipo do assembly onde vivem os handlers
builder.Services.AddMessaging<CreateProductCommandHandler>();

// Com IBus customizado
builder.Services.AddMessaging<CreateProductCommandHandler>(sp => new MeuBusCustomizado(sp));
```

### MessagingOptions

Configurações do pipeline de messaging, passadas via `configure` no `AddMessaging`.

+ `AddOpenBehavior(Type behaviorType)`: adiciona um `IPipelineBehavior<,>` *open generic* ao pipeline (fluente, encadeável). O `ValidationBehavior` já vem incluído automaticamente.

```csharp
builder.Services.AddMessaging<CreateProductCommandHandler>(options =>
{
    options
        .AddOpenBehavior(typeof(LoggingBehavior<,>))
        .AddOpenBehavior(typeof(CachingBehavior<,>));
});
```
