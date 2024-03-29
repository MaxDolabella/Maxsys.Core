using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Maxsys.Core.Exceptions;
using Maxsys.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace Maxsys.Core.Services.Http;

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
    /// Monta HttpRequestMessage, sem inserir authentication (se tiver), insere headers (se tiver) e
    /// retorna o HttpRequestMessage.
    /// </summary>
    protected virtual ValueTask<HttpRequestMessage> GetHttpRequestMessageWithoutAuthenticationAsync(HttpMethod requestMethod, string requestUri, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
    {
        var requestMessage = new HttpRequestMessage(requestMethod, requestUri);

        if (requestHeaders?.Any() == true)
        {
            foreach (var header in requestHeaders)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }
        }

        return ValueTask.FromResult(requestMessage);
    }

    /// <summary>
    /// Monta HttpRequestMessage, insere authentication (se tiver), insere headers (se tiver), insere o body usando JsonContent e
    /// retorna o HttpRequestMessage.
    /// </summary>
    protected virtual async ValueTask<HttpRequestMessage> GetHttpRequestMessageAsync<TJsonContent>(HttpMethod requestMethod, string requestUri, TJsonContent requestJsonContent, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
    {
        var content = JsonContent.Create(requestJsonContent, options: JsonExtensions.JSON_DEFAULT_OPTIONS);
        return await GetHttpRequestMessageWithContentAsync(requestMethod, requestUri, content, requestHeaders, cancellationToken);
    }

    /// <summary>
    /// Monta HttpRequestMessage, insere authentication (se tiver), insere headers (se tiver), insere um HttpContent e
    /// retorna o HttpRequestMessage.
    /// </summary>
    protected virtual async ValueTask<HttpRequestMessage> GetHttpRequestMessageWithContentAsync(HttpMethod requestMethod, string requestUri, HttpContent requestContent, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
    {
        var requestMessage = await GetHttpRequestMessageAsync(requestMethod, requestUri, requestHeaders, cancellationToken);
        requestMessage.Content = requestContent;

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

        return await GetHttpResponseMessageAsync(requestMessage, cancellationToken);
    }

    /// <summary>
    /// Monta a request (com body), envia e retorna a response+content(string).
    /// </summary>
    protected virtual async Task<(HttpResponseMessage Message, string Content)> GetHttpResponseMessageAsync<TJsonContent>(HttpMethod requestMethod, string requestUri, TJsonContent requestJsonContent, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
    {
        var requestMessage = await GetHttpRequestMessageAsync(requestMethod, requestUri, requestJsonContent, requestHeaders, cancellationToken);

        return await GetHttpResponseMessageAsync(requestMessage, cancellationToken);
    }

    /// <summary>
    /// Retorna a response+content(string).
    /// </summary>
    protected virtual async Task<(HttpResponseMessage Message, string Content)> GetHttpResponseMessageAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken = default)
    {
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