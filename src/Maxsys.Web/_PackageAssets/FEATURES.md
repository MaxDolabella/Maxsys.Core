# Maxsys.Web
Biblioteca Maxsys para recursos Web (ASP.NET Core): controller base com respostas padronizadas (`ApiResult`), health check, model binder de JSON e extensions utilitárias. Requer .NET 10.

## Controllers e respostas de API

O padrão de resposta das APIs Maxsys é o `ApiResult` (e variações), definido em `Maxsys.Core` (namespace `Maxsys.Core.Web`). Este pacote fornece o `ApiControllerBase`, que converte `OperationResult`/dados de serviço em respostas `ApiResult` com status code adequado.

### ApiControllerBase

Classe base abstrata para controllers de API (já decorada com `[ApiController]`). Expõe métodos protegidos que montam um `ApiActionResult` a partir do resultado da operação, usando como `Title` o identificador do endpoint (obtido de `HttpContext.GetEndpointIdentifier()` — ver `HttpContextExtensions`).

Membros principais:

- `ApiDataResult<T>(data, okStatus = 200, nullDataStatusCode = 404, resultType = null)` — retorna um dado (ou nulo). Status `200 OK` quando `data != null`, `404 Not Found` quando nulo (ambos configuráveis). Quando `resultType` não é informado, é derivado do status code (1xx = Info, 2xx-3xx = Success, 4xx = Warning, 5xx = Error).
- `ApiListResult<T>(list, resultType = Success)` — retorna um `ListDTO<T>` (lista paginada) com status `200 OK`.
- `ApiOperationResult(OperationResult, okStatus = 200, toNotOkStatusCodeFunc = null)` — resultado de operação sem dado. Se `IsValid`, usa `okStatus`; senão usa `toNotOkStatusCodeFunc`, depois `DefaultNotOkStatusCodeFunc`, e por fim `400 Bad Request`.
- `ApiOperationResult<T>(OperationResult<T>, okStatus = 200, toNotOkStatusCodeFunc = null, returnData = true)` — resultado de operação com dado. `returnData: false` omite o `Data` da resposta.
- `ApiOperationResult<T>(OperationResultCollection<T>, ...)` — múltiplas operações; gera um `ApiMultipleResults<T>` cujo `Data` é uma lista de `ResultItem<T>` (dado + notificações por item).
- `ApiOperationResult(OperationResultCollection, okStatus = 200, toNotOkStatusCodeFunc = null)` — múltiplas operações sem dado.
- `DefaultNotOkStatusCodeFunc` (`Func<IOperationResult, int>?`, virtual) — função padrão do controller para converter um `IOperationResult` inválido em status code. Padrão `null` (usa `400 Bad Request`).

Os métodos `CustomDataResult`/`CustomListResult`/`CustomOperationResult` (que recebiam `endpointTitle` explícito) estão obsoletos — use as versões `Api*Result`.

```csharp
using Maxsys.Web;
using Maxsys.Web.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
public class ContactsController : ApiControllerBase
{
    private readonly IContactService _service;

    public ContactsController(IContactService service) => _service = service;

    // Opcional: status code padrão do controller para operações inválidas
    protected override Func<IOperationResult, int>? DefaultNotOkStatusCodeFunc { get; set; }
        = _ => StatusCodes.Status422UnprocessableEntity;

    [HttpGet("{id:guid}")]
    [ActionIdentifier("CONTACT.GET")]
    public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        ContactDetails? dto = await _service.GetAsync(id, cancellationToken);

        // 200 quando dto != null; 404 quando nulo
        return ApiDataResult(dto);
    }

    [HttpGet]
    [ActionIdentifier("CONTACT.LIST")]
    public async Task<IActionResult> ListAsync([FromQuery] ContactFilter filter, CancellationToken cancellationToken)
    {
        ListDTO<ContactInfo> list = await _service.ToListAsync(filter, cancellationToken);

        return ApiListResult(list);
    }

    [HttpPost]
    [ActionIdentifier("CONTACT.ADD")]
    public async Task<IActionResult> AddAsync([FromBody] ContactCreate dto, CancellationToken cancellationToken)
    {
        OperationResult<Guid> result = await _service.AddAsync(dto, cancellationToken);

        // 201 quando IsValid; senão DefaultNotOkStatusCodeFunc (ou 400)
        return ApiOperationResult(result, okStatus: StatusCodes.Status201Created);
    }

    [HttpDelete("{id:guid}")]
    [ActionIdentifier("CONTACT.DELETE")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        OperationResult result = await _service.DeleteAsync(id, cancellationToken);

        return ApiOperationResult(result);
    }
}
```

Exemplo de resposta JSON (com `ConfigureJsonOptions()` aplicado — camelCase, enum como literal, nulos omitidos):

```json
{
  "maxsysAPI": "MaxsysAPI",
  "title": "CONTACT.GET",
  "statusCode": 200,
  "resultType": "Success",
  "data": { "id": "b2f7...", "name": "Fulano" }
}
```

### ApiActionResult

`ObjectResult` cujo `StatusCode` é o `StatusCode` do `ApiResultBase` recebido. É o tipo de retorno de todos os métodos `Api*Result` do `ApiControllerBase`; raramente precisa ser instanciado manualmente.

```csharp
return new ApiActionResult(new ApiResult("PING", StatusCodes.Status200OK, ResultTypes.Success, notifications: null));
```

### Tipos de resposta (Maxsys.Core.Web)

Os tipos serializados na resposta vivem em `Maxsys.Core` (namespace `Maxsys.Core.Web`), para que clientes possam desserializar sem referenciar ASP.NET Core:

- `ApiResultBase` — base abstrata: `MaxsysAPI` (marcador fixo), `Title` (identificador do endpoint), `StatusCode`, `ResultType` (`ResultTypes`) e `Tag`.
- `ApiResult` — adiciona `Notifications` (lista de `Notification`).
- `ApiResult<T>` — adiciona `Data` (`T?`).
- `ApiMultipleResults<T>` — `ApiResult<IEnumerable<ResultItem<T?>>>` para resultados de múltiplas operações.
- `ResultItem<T>` — item individual: `ResultType` (o mais severo das notificações), `Data` e `Notifications`.
- `ApiResultExtensions.ToOperationResult()` / `ToOperationResult<T>()` — converte um `ApiResult` recebido de volta em `OperationResult` (útil em clientes HTTP; status 404 vira notificação de "item não encontrado").

```csharp
// No cliente HTTP:
var apiResult = await response.Content.ReadFromJsonAsync<ApiResult<ContactDetails>>();
OperationResult<ContactDetails> operationResult = apiResult!.ToOperationResult();
```

## Attributes

### ActionIdentifierAttribute

Define um identificador (título) para uma Action. Esse título é usado como `Title` do `ApiResult` (via `HttpContext.GetEndpointIdentifier()`) e pelo pacote `Maxsys.Swagger` (`ActionIdentifierOperationFilter`) para prefixar o summary do endpoint. Sem o atributo, o identificador cai no formato `Controller:Action`.

```csharp
[HttpGet]
[ActionIdentifier("CONTACT.LIST")]
public async Task<IActionResult> ListAsync(...) { ... }
```

## Model Binding

### FromJsonAttribute

`ModelBinderAttribute` que recebe uma string JSON (tipicamente via query string) e a desserializa no objeto de destino usando o `JsonModelBinder`. Aceita valor nulo ou em branco. O tipo de destino precisa de construtor vazio (exceto arrays/listas).

```csharp
public class ContactFilter
{
    public string? Name { get; set; }
    public List<Guid> Ids { get; set; } = [];
}

// GET api/contacts?filter={"name":"Fulano","ids":["b2f7..."]}
[HttpGet]
public async Task<IActionResult> ListAsync([FromJson] ContactFilter filter, CancellationToken cancellationToken)
{
    ...
}
```

### JsonModelBinder

`IModelBinder` usado pelo `FromJsonAttribute`. Converte o valor bruto do campo em objeto via `FromJson()` (extensão de `Maxsys.Core`); JSON nulo/inválido resulta em objeto vazio (CTOR vazio) ou lista/array vazio.

## Health Check

### UseHealthCheck

Extension de `IEndpointRouteBuilder` que mapeia o endpoint de health checks (padrão `/api/_health`) com um `ResponseWriter` que serializa a resposta como `ApiResult<List<HealthCheckResponse>>` — mesmo formato JSON das demais respostas da API. Status `200 OK` quando saudável; `503 Service Unavailable` (com `ResultType = Warning`) quando `Unhealthy` ou `Degraded`.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<AppDbContext>();

var app = builder.Build();

app.UseHealthCheck(); // GET /api/_health
// ou: app.UseHealthCheck("/healthz");

app.Run();
```

Exemplo de resposta:

```json
{
  "maxsysAPI": "MaxsysAPI",
  "title": "HealthCheck",
  "statusCode": 200,
  "resultType": "Success",
  "data": [
    { "service": "AppDbContext", "status": "Healthy" }
  ]
}
```

### HealthCheckResponse

Record que representa cada verificação no `Data` da resposta: `Service` (nome do check), `Status` (`Healthy`/`Degraded`/`Unhealthy`) e `Description` (opcional).

## Headers

### Headers

Constantes com nomes de cabeçalhos HTTP convencionados para as APIs Maxsys:

- `Headers.RETURN_DATA` (`"mx-return-data"`) — `bool`; indica se a resposta deve incluir `Data`.
- `Headers.STOP_ON_FIRST_FAIL` (`"mx-stop-on-first-fail"`) — `bool`; indica se um lote deve parar na primeira falha.
- `Headers.ENVIRONMENT` (`"mx-environment"`) — nome do environment.
- `Headers.USER` (`"mx-user"`) — usuário da requisição.

```csharp
var returnData = HttpContext.Request.Headers.TryGetValue(Headers.RETURN_DATA, out var value)
    && bool.TryParse(value, out var flag) && flag;

var result = await _service.ImportAsync(dtos, cancellationToken);

return ApiOperationResult(result, returnData: returnData);
```

## Extensions

Todas as extensions abaixo usam *extension members* do C# 14.

### HttpContextExtensions

- `HttpContext.GetEndpointIdentifier()` — obtém o identificador do endpoint atual: o `Title` do `ActionIdentifierAttribute`, quando presente; senão `Controller:Action` (lança `DomainException` se não conseguir resolver as rotas). É a fonte do `Title` nos `Api*Result` do `ApiControllerBase`.

```csharp
var identifier = HttpContext.GetEndpointIdentifier(); // "CONTACT.LIST" ou "Contacts:List"
```

### FormFileExtensions

Extensions para `IFormFile?` (arquivo nulo ou vazio retorna resultado vazio, sem exceção):

- `ToByteArrayAsync(cancellationToken)` — conteúdo como `byte[]`.
- `ToBase64StringAsync(cancellationToken)` — conteúdo como string Base64.
- `ReadContentAsync(cancellationToken)` — conteúdo como texto (linha a linha).

```csharp
[HttpPost("upload")]
public async Task<IActionResult> UploadAsync(IFormFile file, CancellationToken cancellationToken)
{
    byte[] bytes = await file.ToByteArrayAsync(cancellationToken);
    string base64 = await file.ToBase64StringAsync(cancellationToken);
    string text = await file.ReadContentAsync(cancellationToken);
    ...
}
```

### IMvcBuilderExtensions

- `IMvcBuilder.ConfigureJsonOptions(usesJsonStringEnumConverter = true)` — atalho para `AddJsonOptions` com os padrões Maxsys (`JSON_DEFAULT_OPTIONS` de `Maxsys.Core`): camelCase, case-insensitive, ignora ciclos de referência, omite propriedades nulas, encoder relaxado e (opcionalmente) enums serializados como literal (`JsonStringEnumConverter`).

```csharp
builder.Services
    .AddControllers()
    .ConfigureJsonOptions();
```

### MimeTypeExtensions

- `string.GetMimeTypeForFileExtension()` — resolve o MIME type a partir da extensão do arquivo (via `FileExtensionContentTypeProvider`); retorna `application/octet-stream` quando desconhecido.

```csharp
var contentType = "report.xlsx".GetMimeTypeForFileExtension();
// "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"

return File(bytes, contentType, "report.xlsx");
```

## Entry point do assembly

### IWebEntry

Interface vazia usada como referência ao assembly — por exemplo, em `IncludeXmlComments<IWebEntry>()` do pacote `Maxsys.Swagger` ou em scans de assembly.
