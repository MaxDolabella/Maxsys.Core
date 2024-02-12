# Maxsys.Core (Principais Features)

## Attributes

### DefaultSortAttribute
+ Atributo utilizado para indicar a property que será o sort padrão em `QueryableExtensions.ApplySort`

### DependencyInjectionIgnoreAttribute
+ Indica que o objeto não será registrado no ServiceProvider pelos métodos de extensão em `IServiceCollectionExtensions`.

### NotDefaultAttribute
+ Especifica que um campo não pode possuir valor `default` para seu tipo. Lembrando que `null` não é `default`.

### SortableAttribute&lt;T&gt; | SortableAttribute(string)
+ Especifica que um objeto é ordenável através de um enum contendo as colunas ordenáveis.

### SortablePropertyAttribute
+ Usado em um literal de enum *"SortableColumns"* para indicar que esse literal se refere a uma determinada coluna. Dessa maneira, o literal pode ter um nome diferente da propriedade ordenável, esta sendo informada através de `SortablePropertyAttribute.Name`.

### TextAttribute
+ Atributo utilizado para indicar que uma *property* é do tipo `TEXT` no banco, e assim auxiliando na obtenção do *selector* de ordenação.

---
## Exceptions
- `ExternalAPIException`
	- Representa um erro que ocorre ao tentar chamar uma API externa (Workspace por exemplo).
- `InvalidServiceProviderException`
	- Representa um erro que ocorre ao tentar validar um ServiceProvider.
- `NotAllowedOperationException`
	- Representa um erro que ocorre ao se tentar realizar uma operação não permitida.

---
## ListCriteria (Sorts)

Um `SortFilter` contém a informação de qual *property* do DTO será ordenada, bem como sua orientação (`Ascendant`, `Descendant`).
A *property* a ser ordenada pode ser informada através de `SortFilter.Column` (*byte/enum*) ou `SortFilter.ColumnName` (*string/nome da property a ordenar*).

<hr style="border: 1px dashed lightgray;"/>

### Usando `SortFilter.Column` (*byte*)

Ao se usar `Column`, o método de ordenação (`.ApplySort<T>()`) precisa saber que o *byte* informado se refere a determinada *property*.
Por exemplo, para um objeto `CityDTO`, quando `Column == 2`, deve-se ordenar por `City.Abbreviation`. 
Para `.ApplySort()` saber que 2 refere-se a `Abbreviation`, deverá-se informar um *enum* onde cada literal tem seu valor (*byte*) e o nome da *"property ordenável"*.
Esse enum é informado através de um atributo `[Sortable<TEnum>]` no objeto `T` que está sendo ordenado.

#### Exemplo prático

Ao se utilizar `.ApplySort<CityDTO>()`, um enum `CityColumns : byte` é informado em `CityDTO`:

```csharp
public enum CityColumns : byte
{
	None = 0,
	Name, 
	Abbreviation,
	// demais literais...
}
```

```csharp
[Sortable<CityColumns>] // Atributo indicando que CityDTO utiliza o enum CityColumns
public class CityDTO
{
    // Properties Id, Name, Abbreviation e demais...
}

```

```csharp
var sortByAbbreviation = new SortFilter(2, Direction.Ascendant);
// ou
// var sortByAbbreviation = new SortFilter((byte)CityColumns.Abbreviation, Direction.Ascendant);

ListCriteria criteria = new ()
{
    Sorts = new [sortByAbbreviation]
};

// ordenação é realizada
cityQueryable.ApplySort();
```

#### Considerações

+ Se o objeto `T` não tiver um atributo `[Sortable<TEnum>]`, então nenhuma ordenação será feita.
+ :warning: Se nenhuma *property* do objeto `T` for igual ao nome do literal de `TEnum`, então uma *exception* será lançada.

No entanto, é possível informar o *"caminho da property"* através de um atributo `[SortableProperty("PropertyName")]` no literal.
Considerando o exemplo anterior com a utilização do enum abaixo, o resultado será o mesmo:

```csharp
public enum CityColumns : byte
{
    None = 0,
    Name,
	
    [SortableProperty("Abbreviation")]
    CucaBeludo,
    // demais literais...
}
```

#### Propriedades aninhadas

Supondo que `CityDTO` contém uma property `StateDTO State` e `StateDTO` contém uma property *Name*, 
é possível ordernar `CityDTO` por `State.Name`. Considere os objetos abaixo:

```csharp
[Sortable<CityColumns>]
public class CityDTO
{
    // Properties Id, Name, Abbreviation e demais...
	
    public StateDTO State { get; set; }
}

public class StateDTO
{
    // Properties Id, Name, Abbreviation e demais...
	
    public CountryDTO Country { get; set; }	
}

public class CountryDTO
{
    // Properties Id, Name, Abbreviation e demais...
}
```

A definição das propriedades aninhadas no enum poderá ser feita de duas formas:
+ Utilizando o atributo `[SortableProperty]`;
+ Utilizando a convenção de literal com `__` para cada property aninhada;

##### Exemplo utilizando o atributo `SortableProperty`:

```csharp
public enum CityColumns : byte
{
    None = 0,
    Name,
    Abbreviation,
	
    [SortableProperty("State.Name")]
    State,
	
    [SortableProperty("State.Country.Name")]
    Country,
    // demais literais...
}
```

##### Exemplo utilizando convenção `__`:

```csharp
public enum CityColumns : byte
{
    None = 0,
    Name,
    Abbreviation,
	
    State__Name,
    State__Country__Name,
	
    // demais literais...
}
```

O resultado da ordenação dos dois *enumns* será o mesmo.

<hr style="border: 1px dashed lightgray;"/>

### Usando `SortFilter.ColumnName` (*string*)

Ao se usar `ColumnName`, o método de ordenação (`.ApplySort<T>()`) irá procurar uma property em `T` com o nome informado.
Não é necessário criar enum para o DTO, o que por conseguinte isenta uso de atributo [Sortable<TEnum>] no DTO.

Por exemplo, para um objeto `CityDTO`, quando `ColumnName == "Abbreviation"`, a ordenação será por `City.Abbreviation`.

#### Exemplo prático

```csharp
public class CityDTO
{
    // Properties Id, Name, Abbreviation e demais...
}

```

```csharp
var sortByAbbreviation = new SortFilter("Abbreviation", Direction.Ascendant);

ListCriteria criteria = new ()
{
    Sorts = new [sortByAbbreviation]
}

// ordenação é realizada
cityQueryable.ApplySort();
```

#### Considerações

:warning: Se o objeto `T` não tiver uma *property* igual ao nome do informado em `ColumnName`, então uma *exception* será lançada.

#### Propriedades aninhadas

Supondo que `CityDTO` contém uma property `StateDTO State` e `StateDTO` contém uma property *Name*, 
é possível ordernar `CityDTO` por `State.Name`. Considere os objetos abaixo:

```csharp
public class CityDTO
{
    // Properties Id, Name, Abbreviation e demais...
	
    public StateDTO State { get; set; }
}

public class StateDTO
{
    // Properties Id, Name, Abbreviation e demais...
	
    public CountryDTO Country { get; set; }	
}

public class CountryDTO
{
    // Properties Id, Name, Abbreviation e demais...
}
```

A definição de ordenação das propriedades aninhadas poderá ser feita passando o *"caminho das properties"* separados por `.`(ponto) em `ColumnName`;

##### Exemplo 

```csharp
var sortByCountry = new SortFilter("State.Country.Name", Direction.Ascendant);
var sortByState = new SortFilter("State.Name", Direction.Ascendant);
var sortByAbbreviation = new SortFilter("Abbreviation", Direction.Ascendant);

ListCriteria criteria = new ()
{
    Sorts = new [sortByCountry, sortByState, sortByAbbreviation]
};

// ordenação é realizada
cityQueryable.ApplySort();
```

---
## Algumas Outras Features

### SearchTerm

<details>
	<summary>Exemplo de <code>SearchTerm.ToExpression()</code> usado em <code>Filter&lt;Person&gt;.SetExpression()</code></summary>

```cs

if (!string.IsNullOrWhiteSpace(Search?.Term))
{
    var expression = Search.ToExpression<Person>(entity => new[]
        {
            entity.SearchName,
            entity.LogonName,
            entity.PhoneNumber,
            entity.FaxNumber,
            entity.EmailAddress
        });

    AddExpression(expression);
}

	
```
</details>

### HttpClientBase

<details>
	<summary>Código</summary>

```cs

public abstract class HttpClientBase : ServiceBase
{
    protected readonly ILogger _logger;
    protected readonly HttpClient _httpClient;
    private readonly string _apiPrefix;

    protected HttpClientBase(ILogger logger, IHttpClientFactory httpClientFactory)
        : this(logger, string.Empty, httpClientFactory)
    { }

    protected HttpClientBase(
        ILogger logger,
        string apiPrefix,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _apiPrefix = apiPrefix;
        _httpClient = httpClientFactory.CreateClient();
    }

    #region Protected Methods

    #region RequestMessage

    /// <remarks>
    /// Se não houver autenticação:
    /// <code>
    /// return null;
    /// </code>
    /// <para/>
    /// Se houver autenticação (token):
    /// <code>
    /// return new("Bearer", await _tokenProvider.GetTokenAsync(cancellationToken));
    /// </code>
    /// </remarks>
    protected abstract ValueTask<AuthenticationHeaderValue?> AddAuthTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Monta HttpRequestMessage, insere authentication (se tiver), insere headers (se tiver) e
    /// retorna o HttpRequestMessage.
    /// </summary>
    protected virtual async ValueTask<HttpRequestMessage> GetHttpRequestMessageAsync(HttpMethod requestMethod, string requestUri, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(requestMethod, requestUri);

        var authenticationHeaderValue = await AddAuthTokenAsync(cancellationToken);
        if (authenticationHeaderValue is not null)
        {
            requestMessage.Headers.Authorization = authenticationHeaderValue;
        }

        if (requestHeaders?.Any() == true)
        {
            foreach (var header in requestHeaders)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }
        }

        return requestMessage;
    }

    /// <summary>
    /// Monta HttpRequestMessage, insere authentication (se tiver), insere headers (se tiver), insere o body usando JsonContent e
    /// retorna o HttpRequestMessage.
    /// </summary>
    protected virtual async ValueTask<HttpRequestMessage> GetHttpRequestMessageAsync<TContent>(HttpMethod requestMethod, string requestUri, TContent requestContent, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
    {
        var requestMessage = await GetHttpRequestMessageAsync(requestMethod, requestUri, requestHeaders, cancellationToken);
        requestMessage.Content = JsonContent.Create(requestContent, options: JsonExtensions.JSON_DEFAULT_OPTIONS);

        return requestMessage;
    }

    #endregion RequestMessage

    #region ResponseMessage

    /// <summary>
    /// Monta a request, envia e retorna a response+content(string).
    /// </summary>
    protected virtual async Task<(HttpResponseMessage Message, string Content)> GetHttpResponseMessageAsync(HttpMethod requestMethod, string requestUri, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
    {
        var requestMessage = await GetHttpRequestMessageAsync(requestMethod, requestUri, requestHeaders, cancellationToken);
        var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
        var responseContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

        return (responseMessage, responseContent);
    }

    /// <summary>
    /// Monta a request (com body), envia e retorna a response+content(string).
    /// </summary>
    protected virtual async Task<(HttpResponseMessage Message, string Content)> GetHttpResponseMessageAsync<TContent>(HttpMethod requestMethod, string requestUri, TContent requestContent, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
    {
        var requestMessage = await GetHttpRequestMessageAsync(requestMethod, requestUri, requestContent, requestHeaders, cancellationToken);
        var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
        var responseContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

        return (responseMessage, responseContent);
    }

    #endregion ResponseMessage

    #region ApiResponse

    /// <summary>
    /// Monta a request, envia, verifica se a response contém uma prop 'title' iniciada com o ApiPrefix e retorna a response+content(string).
    /// </summary>
    /// <exception cref="ExternalAPIException" />
    protected virtual async Task<(HttpResponseMessage Message, string Content)> GetApiResponseAsync(HttpMethod requestMethod, string requestUri, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
    {
        var response = await GetHttpResponseMessageAsync(requestMethod, requestUri, requestHeaders, cancellationToken);

        ThrowExternalAPIExceptionIfResponseIsNoApiResultWithTitlePrefix(response);

        return response;
    }

    /// <summary>
    /// Monta a request com body, envia, verifica se a response contém uma prop 'title' iniciada com o ApiPrefix e retorna a response+content(string).
    /// </summary>
    /// <exception cref="ExternalAPIException" />
    protected virtual async Task<(HttpResponseMessage Message, string Content)> GetApiResponseAsync<TContent>(HttpMethod requestMethod, string requestUri, TContent requestContent, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
    {
        var response = await GetHttpResponseMessageAsync(requestMethod, requestUri, requestContent, requestHeaders, cancellationToken);

        ThrowExternalAPIExceptionIfResponseIsNoApiResultWithTitlePrefix(response);

        return response;
    }

    /// <summary>
    /// Primeiramente, verifica se há um erro de validation (ex, quando se espera um param não passado).<br/>
    /// Em seguida, verifica se o json do resultado contém $.title iniciado com PREFIX.
    /// </summary>
    /// <exception cref="ExternalAPIException"></exception>
    protected virtual void ThrowExternalAPIExceptionIfResponseIsNoApiResultWithTitlePrefix((HttpResponseMessage Message, string Content) response)
    {
        var jsonDoc = JsonDocument.Parse(response.Content);

        if (!jsonDoc.RootElement.TryGetProperty("title", out var titleProperty))
        {
            _logger.LogError("ExternalAPIException: {reasonPhrase}", response.Content);
            throw new ExternalAPIException(response.Message.StatusCode, response.Content);
        }

        // One or more validation errors occurred.
        if (titleProperty.ValueEquals("One or more validation errors occurred."))
        {
            /* Se chegou aqui, provavelmente a response está nesse formato:
            {
              "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
              "title": "One or more validation errors occurred.",
              "status": 400,
              "traceId": "00-6883c0d3459086c9ec5389bf5eb154ee-cb27797253ef54cb-00",
              "errors": {
                "awb": [
                  "The awb field is required."
                ],
                "$.masterConsignment.includedHouseConsignment.weightChargeAmount.currencyID": [
                  "The JSON value could not be converted to System.String. Path: $.masterConsignment.includedHouseConsignment.weightChargeAmount.currencyID | LineNumber: 0 | BytePositionInLine: 1036."
                ]
              }
            }
            */

            var errors = string.Join("\r\n", jsonDoc.RootElement.GetProperty("errors")
                .EnumerateObject()
                .Select(e => e.Value[0].ToString()));

            _logger.LogError("ExternalAPIException: {reasonContent}", response.Content);
            throw new ExternalAPIException(response.Message.StatusCode, errors);
        }

        // Esperado PREFIX
        if (!titleProperty.GetString()?.StartsWith(_apiPrefix) == true)
        {
            _logger.LogError("ExternalAPIException: {reasonContent}", response.Content);
            throw new ExternalAPIException(response.Message.StatusCode, response.Content);
        }
    }

    #endregion ApiResponse

    #endregion Protected Methods
}

```
</details>

<details>
	<summary>Exemplo de uso</summary>

```cs

// appsettings
public class MovieSettings
{
    public required string ApiKey { get; set; }
    public required string BaseURL { get; set; }
}

public sealed class MovieService : HttpClientBase, IMovieService
{
    private readonly MovieSettings _settings;
    private readonly IMovieTokenProvider _tokenProvider;

    public MovieService(ILogger<MovieService> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory, IMovieTokenProvider tokenProvider, IOptions<MovieSettings> options)
        : base(logger, httpClientFactory)
    {
        _settings = options.Value;
        _tokenProvider = tokenProvider;
    }

    public async Task<OperationResult<MovieDTO>> GetExemplo01Async(Guid id, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_settings.BaseURL}/movies/{id}";
        var requestHeaders = GetCommonHeaders();

        var requestMessage = await GetHttpRequestMessageAsync(HttpMethod.Get, requestUri, requestHeaders, cancellationToken);
        var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
        var responseContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

        if (responseMessage.IsSuccessStatusCode)
        {
            var result = responseContent.FromJson<MovieDTO>();
            return new(result);
        }
        else if (responseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            return new(GenericMessages.ITEM_NOT_FOUND);
        }
        /* Outro erro tratável
        else if (responseMessage.StatusCode == OtherStatusCode)
        {
            var result = response.Content.FromJson<OtherExpectedObject>();
            return new(result.SomeMessage);
        }
        */
        else
        {
            return new(new Notification("SomeErrorMessage", details: responseContent ));

            /* Ou throw new ExternalAPIException(responseMessage.StatusCode, responseContent); */
        }
    }

    public async Task<OperationResult<MovieDTO>> GetExemplo02Async(Guid id, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_settings.BaseURL}/movies?id={id}";
        var requestHeaders = new Dictionary<string, string>()
        {
            [Headers.SOME_HEADER_KEY] = "cuca-beludo",
        };

        var response = await GetHttpResponseMessageAsync(HttpMethod.Get, requestUri, requestHeaders, cancellationToken);
        // ou var (responseMessage, responseContent) = await GetHttpResponseMessageAsync(HttpMethod.Get, requestUri, requestHeaders, cancellationToken);

        try
        {
            response.Message.EnsureSuccessStatusCode();

            // Response Ok
            return new(response.Content.FromJson<MovieDTO>());
        }

        #region Status code 500

        catch (HttpRequestException ex) when (ex.StatusCode is HttpStatusCode.InternalServerError)
        {
            _logger.LogError(ex, "Status Code: {StatusCode}", ex.StatusCode);

            return new(new Notification("EXTERNAL_SERVICE_UNAVAILABLE") { Tag = response.Content });
        }

        #endregion Status code 500

        #region Status Code 401 / 403

        catch (HttpRequestException ex) when (ex.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
        {
            _logger.LogError(ex, "Status Code: {StatusCode}", ex.StatusCode);

            return new(new Notification("NOT_AUTHORIZED") { Tag = response.Content });
        }

        #endregion Status Code 401 / 403

        #region Status Code 404

        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogError(ex, "Status Code: {StatusCode}", ex.StatusCode);

            return new(new Notification("NOT_FOUND") { Tag = response.Content });
        }

        #endregion Status Code 404

        #region Outros status 4xx

        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Status Code: {StatusCode}", ex.StatusCode);

            var error = response.Content.FromJson<SomeResponseErrorExpectedObject>(response.Content);

            return new(new Notification("ANOTHER_ERROR_MESSAGE") { Tag = response.Content });
        }

        #endregion Outros status 4xx

        catch (Exception ex)
        {
            _logger.LogError(ex, "UNEXPECTED_ERROR");
            return new(new Notification(ex, "UNEXPECTED_ERROR"));
            /* Ou throw new ExternalAPIException(response.Message.StatusCode, response.Content); */
        }
    }

    public async Task<OperationResult<MovieDTO>> GetFromExpectedMaxsysApiResultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_settings.BaseURL}/movies/{id}";
        
        // Se o json do response content não tiver uma prop 'title' iniciada com um prefixo passado no CTOR.base 
        // (ex, :base(logger, "MOVIES_API", httpClientFactory) e content json: $.title == "MOVIES_API-GET_MOVIE" ou $.title == "One or more validation errors occurred."),
        // então uma ExternalAPIException será lançada com o StatusCode+Content(string)
        var response = await GetApiResponseAsync(HttpMethod.Get, requestUri, requestHeaders: null, cancellationToken);

        if (response.Message.IsSuccessStatusCode)
        {
            var apiResult = response.Content.FromJson<ApiDataResult<MovieDTO>>();
            return new(apiResult.Result!.Data!);
        }
        else
        {
            var apiResult = response.Content.FromJson<ApiResult<OperationResult>>();
            return new(apiResult.Result!.Notifications);
        }
    }

    private static IDictionary<string, string> GetCommonHeaders()
    {
        var headers = new Dictionary<string, string>()
        {
            [Headers.API_HEADER_KEY] = ApiKey,
        };

        return headers;
    }

    // Caso não haja autenticação, usar a linha abaixo:
    // protected override ValueTask<AuthenticationHeaderValue?> AddAuthTokenAsync(CancellationToken cancellationToken = default) => ValueTask.FromResult(default(AuthenticationHeaderValue?));

    protected override async ValueTask<AuthenticationHeaderValue?> AddAuthTokenAsync(CancellationToken cancellationToken = default)
    {
        return new("Bearer", await _tokenProvider.GetTokenAsync(cancellationToken));
    }
}
```
</details>



## Extensions (Principais)

### JsonExtensions
- `.FromJson()`
	<details>
		<summary>Exemplo de uso</summary>

	```cs

	string json = """"{"id":123,"name":"Giuseppe Kadura"}"""";

	Person person = json.FromJson<Person>();
	Person person = json.FromJson<Person>(Person.Default); // caso json==null, retorna default
	Person person = json.FromJson(typeof(Person));
	```
	</details>

- `.ToJson()`
	<details>
		<summary>Exemplo de uso</summary>

	```cs
	Person person = new()
	{
		Id = 123,
		Name = "Giuseppe Kadura"
	};

	string json = person.ToJson();

	// json = {"id":123,"name":"Giuseppe Kadura"}
	```
	</details>

### EnumExtensions
- `.ToFriendlyName()`
	<details>
		<summary>Exemplo de uso</summary>

	```cs
	public enum SampleEnum : byte
	{
	    [Description("Este é o tipo A")]
	    TipoA = 1,

	    [Description("Este é o tipo B")]
	    TipoB,

	    // Sem description
	    TipoC = 99
	}

	var enumA = SampleEnum.TipoA;
	var enumC = SampleEnum.TipoC;
	var enumNull = default(SampleEnum?);
	var notEnum = (SampleEnum)77;

	Console.WriteLine(enumA.ToFriendlyName());            // "Este é o tipo A"
	Console.WriteLine(enumC.ToFriendlyName());            // "TipoC"
	Console.WriteLine(enumNull.ToFriendlyName(""));       // ""
	Console.WriteLine(enumNull.ToFriendlyName());         // null
	Console.WriteLine(enumNull.ToFriendlyName("Nenhum")); // "Nenhum"
	Console.WriteLine(notEnum.ToFriendlyName());          // "77"
	```
	</details>

- `.ToEnum()`
	<details>
		<summary>Exemplo de uso</summary>

	```cs
	var enumA = "TipoA".ToEnum<SampleEnum>();
	var enumB = "Este é o tipo B".ToEnum<SampleEnum>();
	var notEnum = "TipoD".ToEnum<SampleEnum>();

	Console.WriteLine((byte?)enumA);   // 1
	Console.WriteLine((byte?)enumB);   // 2
	Console.WriteLine((byte?)notEnum); // null
	```
	</details>

### DateTimeExtensions
- IsBetween
	<details>
		<summary>Exemplo de uso</summary>
	<p>
	Checa se uma data está entre duas datas (modo inclusivo).
	</p>

	```cs
	// Natal de 2023
	var dateToCheck = new DateTime(2023, 12, 25);

	var year2023 = new Period(
		new DateTime(2023, 01, 01),
		new DateTime(2023, 12, 31));

	var decade90 = new Period(
		new DateTime(1990, 01, 01),
		new DateTime(1999, 12, 31));

	var after2000 = new Period(
		new DateTime(2000, 01, 01),
		null);

	var before2000 = new Period(
		null,
		new DateTime(1999, 12, 31));


	var isYear2023 = dateToCheck.IsBetween(year2023);
	var isDecade90 = dateToCheck.IsBetween(decade90);
	var isAfter2000 = dateToCheck.IsBetween(after2000);
	var isBefore2000 = dateToCheck.IsBetween(before2000);
	
	Console.WriteLine(isYear2023);    // true
	Console.WriteLine(isDecade90);    // false
	Console.WriteLine(isAfter2000);   // true
	Console.WriteLine(isBefore2000);  // false
	```

	Além desse método, existem mais overloads.
	</details>

### IServiceCollectionExtensions
- AddImplementations
	<details>
		<summary>Exemplo de uso</summary>
	<p>
	No exemplo abaixo, busca-se as interfaces do assembly de CoreEntry
	cujo nome termina em "Provider" e que herdam de IService (ex.: IDataProvider : IService),
	e então busca para cada uma, sua implementação no assembly de DataEntry.
	Em seguida, adiciona os objetos ao ServiceCollection (interface x implementação).
	</p>

	```cs
	services.AddImplementations<IService, CoreEntry, DataEntry>("Provider");
	```

	Além desse método, existem mais overloads.
	</details>