using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Maxsys.Core.Extensions;
using Maxsys.Core.Services;
using Maxsys.Core.Web;

namespace Maxsys.Core.Http;

public abstract class HttpServiceBase : ServiceBase
{
    protected readonly HttpClient _httpClient;
    private readonly string? _apiPrefix;

    protected HttpServiceBase(IHttpClientFactory httpClientFactory, string? apiPrefix)
    {
        _httpClient = httpClientFactory.CreateClient();
        _apiPrefix = apiPrefix;
    }

    protected HttpServiceBase(IHttpClientFactory httpClientFactory) 
        : this(httpClientFactory, null)
    { }

    #region Events

    // Handlers

    public event SendingEventHandler? Sending;

    public event SentEventHandler? Sent;

    public event MaxsysApiResponseInvalidEventHandler? MaxsysApiResponseInvalid;

    public event MaxsysApiResponseValidEventHandler? MaxsysApiResponseValid;

    // Hooks
    protected async Task OnSending(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
    {
        if (Sending is not null)
        {
            foreach (var eventHandler in Sending.GetInvocationList().Cast<SendingEventHandler>())
            {
                if (eventHandler is null)
                    continue;

                await eventHandler(this, new SendingEventArgs(requestMessage), cancellationToken);
            }
        }
    }

    protected async Task OnSent(HttpResponseMessage responseMessage, CancellationToken cancellationToken)
    {
        if (Sent is not null)
        {
            foreach (var eventHandler in Sent.GetInvocationList().Cast<SentEventHandler>())
            {
                if (eventHandler is null)
                    continue;

                await eventHandler(this, new SentEventArgs(responseMessage), cancellationToken);
            }
        }
    }

    protected async Task OnMaxsysApiResponseInvalid(MaxsysApiValidationResult validationResult, CancellationToken cancellationToken)
    {
        if (MaxsysApiResponseInvalid is not null)
        {
            foreach (var eventHandler in MaxsysApiResponseInvalid.GetInvocationList().Cast<MaxsysApiResponseInvalidEventHandler>())
            {
                if (eventHandler is null)
                    continue;

                await eventHandler(this, new MaxsysApiResponseInvalidEventArgs(validationResult), cancellationToken);
            }
        }
    }

    protected async Task OnMaxsysApiResponseValid(HttpResponseMessage httpResponseMessage, string responseContent, CancellationToken cancellationToken)
    {
        if (MaxsysApiResponseValid is not null)
        {
            foreach (var eventHandler in MaxsysApiResponseValid.GetInvocationList().Cast<MaxsysApiResponseValidEventHandler>())
            {
                if (eventHandler is null)
                    continue;

                await eventHandler(this, new MaxsysApiResponseValidEventArgs(httpResponseMessage, responseContent), cancellationToken);
            }
        }
    }

    protected virtual void UnsubscribeEvents()
    {
        Sending = null;
        Sent = null;
        MaxsysApiResponseInvalid = null;
        MaxsysApiResponseValid = null;
    }

    #endregion Events

    #region Helpers

    /// <summary>
    /// Adiciona um <see cref="HttpContent"/> ao <see cref="HttpRequestMessage"/>
    /// </summary>
    /// <param name="requestMessage"></param>
    /// <param name="requestContent"></param>
    protected static void AddContent(HttpRequestMessage requestMessage, HttpContent requestContent) => requestMessage.Content = requestContent;

    protected static void AddHeaders(HttpRequestMessage requestMessage, IDictionary<string, string> requestHeaders)
    {
        if (requestHeaders.Count == 0)
            return;

        foreach (var header in requestHeaders)
        {
            requestMessage.Headers.Add(header.Key, header.Value);
        }
    }

    protected static HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, string requestUri, IDictionary<string, string>? requestHeaders, HttpContent? requestContent)
    {
        // Message
        var requestMessage = new HttpRequestMessage(method, requestUri);

        // Headers
        if (requestHeaders is not null)
        {
            AddHeaders(requestMessage, requestHeaders);
        }

        // Content
        if (requestContent is not null)
        {
            AddContent(requestMessage, requestContent);
        }

        return requestMessage;
    }

    protected static HttpContent CreateJsonContent<T>(T requestBody)
    {
        return JsonContent.Create(requestBody, options: JsonExtensions.JSON_DEFAULT_OPTIONS);
    }

    protected Task<OperationResult> GetResultAsync(string requestUri, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
    {
        return GetMaxsysApiAsync(HttpMethod.Get, requestUri, requestHeaders, null, cancellationToken);
    }

    protected Task<OperationResult<T>> GetDeleteResultAsync<T>(string requestUri, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
    {
        return GetMaxsysApiAsync<T>(HttpMethod.Delete, requestUri, requestHeaders, null, cancellationToken);
    }

    protected Task<OperationResult> GetDeleteResultAsync(string requestUri, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
    {
        return GetMaxsysApiAsync(HttpMethod.Delete, requestUri, requestHeaders, null, cancellationToken);
    }

    #endregion Helpers

    #region SEND

    protected Task<HttpResponseMessage> SendAsync(HttpMethod method, string requestUri, CancellationToken cancellationToken = default)
    {
        return SendAsync(CreateHttpRequestMessage(method, requestUri, null, null), cancellationToken);
    }

    protected Task<HttpResponseMessage> SendAsync(HttpMethod method, string requestUri, HttpContent requestContent, CancellationToken cancellationToken = default)
    {
        return SendAsync(CreateHttpRequestMessage(method, requestUri, null, requestContent), cancellationToken);
    }

    protected Task<HttpResponseMessage> SendAsync(HttpMethod method, string requestUri, IDictionary<string, string> requestHeaders, CancellationToken cancellationToken = default)
    {
        return SendAsync(CreateHttpRequestMessage(method, requestUri, requestHeaders, null), cancellationToken);
    }

    protected Task<HttpResponseMessage> SendAsync(HttpMethod method, string requestUri, IDictionary<string, string> requestHeaders, HttpContent requestContent, CancellationToken cancellationToken = default)
    {
        return SendAsync(CreateHttpRequestMessage(method, requestUri, requestHeaders, requestContent), cancellationToken);
    }

    protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken = default)
    {
        await OnSending(requestMessage, cancellationToken);

        var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);

        await OnSent(responseMessage, cancellationToken);

        return responseMessage;
    }

    #endregion SEND

    #region MAXSYS API RESPONSE

    protected async Task<OperationResult<T>> GetMaxsysApiAsync<T>(HttpMethod httpMethod, string requestUri, IDictionary<string, string>? requestHeaders, HttpContent? requestContent, CancellationToken cancellationToken = default)
    {
        // events: OnSending -> OnSent
        var response = await SendAsync(CreateHttpRequestMessage(httpMethod, requestUri, requestHeaders, requestContent), cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        // events: OnMaxsysApiResponseInvalid
        var validationApiResponseResult = ValidateMaxsysApiResponse(response, responseContent);
        if (!validationApiResponseResult.IsValid)
        {
            await OnMaxsysApiResponseInvalid(validationApiResponseResult, cancellationToken);

            return validationApiResponseResult.ToOperationResult<T>();
        }

        // events: OnMaxsysApiResponseValid
        await OnMaxsysApiResponseValid(response, responseContent, cancellationToken);

        return responseContent.TryFromJson<ApiResult<T>>(out var apiResult, out var notification)
            ? apiResult.ToOperationResult()
            : Result.FromNotifications<T>([notification]);
    }

    protected async Task<OperationResult> GetMaxsysApiAsync(HttpMethod httpMethod, string requestUri, IDictionary<string, string>? requestHeaders, HttpContent? requestContent, CancellationToken cancellationToken = default)
    {
        // events: OnSending -> OnSent
        var response = await SendAsync(CreateHttpRequestMessage(httpMethod, requestUri, requestHeaders, requestContent), cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        // events: OnMaxsysApiResponseInvalid
        var validationApiResponseResult = ValidateMaxsysApiResponse(response, responseContent);
        if (!validationApiResponseResult.IsValid)
        {
            await OnMaxsysApiResponseInvalid(validationApiResponseResult, cancellationToken);

            return validationApiResponseResult.ToOperationResult();
        }

        // events: OnMaxsysApiResponseValid
        await OnMaxsysApiResponseValid(response, responseContent, cancellationToken);

        return responseContent.TryFromJson<ApiResult>(out var apiResult, out var notification)
            ? apiResult.ToOperationResult()
            : Result.FromNotifications([notification]);
    }

    protected Task<OperationResult<T>> GetResultAsync<T>(string requestUri, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
    {
        return GetMaxsysApiAsync<T>(HttpMethod.Get, requestUri, requestHeaders, null, cancellationToken);
    }

    protected Task<OperationResult<T>> GetPostResultAsync<T>(string requestUri, IDictionary<string, string>? requestHeaders, HttpContent? requestContent, CancellationToken cancellationToken = default)
    {
        return GetMaxsysApiAsync<T>(HttpMethod.Post, requestUri, requestHeaders, requestContent, cancellationToken);
    }

    protected Task<OperationResult<T>> GetPutResultAsync<T>(string requestUri, IDictionary<string, string>? requestHeaders, HttpContent? requestContent, CancellationToken cancellationToken = default)
    {
        return GetMaxsysApiAsync<T>(HttpMethod.Put, requestUri, requestHeaders, requestContent, cancellationToken);
    }

    protected Task<OperationResult<T>> GetDeleteResultAsync<T>(string requestUri, IDictionary<string, string>? requestHeaders, HttpContent? requestContent, CancellationToken cancellationToken = default)
    {
        return GetMaxsysApiAsync<T>(HttpMethod.Delete, requestUri, requestHeaders, requestContent, cancellationToken);
    }

    protected Task<OperationResult> GetPostResultAsync(string requestUri, IDictionary<string, string>? requestHeaders, HttpContent? requestContent, CancellationToken cancellationToken = default)
    {
        return GetMaxsysApiAsync(HttpMethod.Post, requestUri, requestHeaders, requestContent, cancellationToken);
    }

    protected Task<OperationResult> GetPutResultAsync(string requestUri, IDictionary<string, string>? requestHeaders, HttpContent? requestContent, CancellationToken cancellationToken = default)
    {
        return GetMaxsysApiAsync(HttpMethod.Put, requestUri, requestHeaders, requestContent, cancellationToken);
    }

    protected Task<OperationResult> GetDeleteResultAsync(string requestUri, IDictionary<string, string>? requestHeaders, HttpContent? requestContent, CancellationToken cancellationToken = default)
    {
        return GetMaxsysApiAsync(HttpMethod.Delete, requestUri, requestHeaders, requestContent, cancellationToken);
    }

    #endregion MAXSYS API RESPONSE

    #region Private

    private MaxsysApiValidationResult ValidateMaxsysApiResponse(HttpResponseMessage responseMessage, string responseContent)
    {
        try
        {
            // Validação de status não autorizado
            if (responseMessage.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            {
                return MaxsysApiValidationResult.CreateInvalidResult(responseMessage.StatusCode, "Unauthorized");
            }

            // Validação de conteúdo vazio
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                return MaxsysApiValidationResult.CreateInvalidResult(responseMessage.StatusCode, "No Content Response");
            }

            // Validação de tipo de conteúdo
            if (responseMessage.Content.Headers.ContentType?.MediaType?.Contains("json") != true)
            {
                return MaxsysApiValidationResult.CreateInvalidResult(responseMessage.StatusCode, "Content is not a valid json.", responseContent);
            }

            // NOTE: A partir desse ponto já consigo ler o json.
            // Parse e validação do JSON
            using var jsonDoc = JsonDocument.Parse(responseContent);
            var root = jsonDoc.RootElement;

            #region Old Title Validation

            // if (!root.TryGetProperty("title", out var titleProperty))
            // {
            //     return MaxsysApiValidationResult.CreateInvalidResult(responseMessage.StatusCode, "Not valid Maxsys API response: No 'title' property found.", responseContent);
            // }
            //
            // // Validação de erros de validação
            // if (titleProperty.ValueEquals("One or more validation errors occurred."))
            // {
            //     /* Se chegou aqui, provavelmente a response está nesse formato:
            //     {
            //       "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            //       "title": "One or more validation errors occurred.",
            //       "status": 400,
            //       "traceId": "00-6883c0d3459086c9ec5389bf5eb154ee-cb27797253ef54cb-00",
            //       "errors": {
            //         "awb": [
            //           "The awb field is required."
            //         ],
            //         "$.masterConsignment.includedHouseConsignment.weightChargeAmount.currencyID": [
            //           "The JSON value could not be converted to System.String. Path: $.masterConsignment.includedHouseConsignment.weightChargeAmount.currencyID | LineNumber: 0 | BytePositionInLine: 1036."
            //         ]
            //       }
            //     }
            //     */
            //
            //     // TODO<testar> E se o retorno tiver title "One or more validation errors occurred." mas não tiver a prop "errors"?
            //     var errors = GetErrorsFromPropertyInJsonElement(root);
            //
            //     return MaxsysApiValidationResult.CreateInvalidResult(responseMessage.StatusCode, $"One or more validation errors occurred:\n{errors}.", responseContent);
            // }
            //
            // // Validação do prefixo
            // if (!TitleElementStartsWith(titleProperty, prefix))
            // {
            //     return MaxsysApiValidationResult.CreateInvalidResult(responseMessage.StatusCode, "Expected API response prefix identifier is invalid.", responseContent);
            // }

            #endregion Old Title Validation

            // RFC 7231: One or more validation errors occurred
            var hasTitle = root.TryGetProperty("title", out var title);
            if (hasTitle && title.ValueEquals("One or more validation errors occurred."))
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

                // TODO<testar> E se o retorno tiver title "One or more validation errors occurred." mas não tiver a prop "errors"?
                var errors = GetErrorsFromPropertyInJsonElement(root);

                return MaxsysApiValidationResult.CreateInvalidResult(responseMessage.StatusCode, $"One or more validation errors occurred:\n{errors}.", responseContent);
            }

            // TODO excluir trecho abaixo quando todas APIs Maxsys se tiverem versão core < 49 (ApiResultBase.MAXSYS_API)
            /* WARNING: Compatibilidade com APIs antigas que se utilizam de apiPrefix:
                Se existe _apiPrefix, é porque a API chamada está em uma versão core < 49. Logo, não utiliza ApiResultBase.MAXSYS_API.
            */
            if (!string.IsNullOrWhiteSpace(_apiPrefix) && hasTitle && TitleElementStartsWith(title, _apiPrefix))
            {
                return MaxsysApiValidationResult.CreateValidResult();
            }

            return root.TryGetProperty(ApiResultBase.MAXSYS_API, out var _)
                ? MaxsysApiValidationResult.CreateValidResult()
                : MaxsysApiValidationResult.CreateInvalidResult(responseMessage.StatusCode, "Maxsys API validation fails: expected identifier not found.", responseContent);
        }
        catch (Exception ex)
        {
            return MaxsysApiValidationResult.CreateInvalidResult(responseMessage.StatusCode, "Error", responseContent, ex);
        }
    }

    private static string GetErrorsFromPropertyInJsonElement(JsonElement element)
    {
        return string.Join('\n', element
            .GetProperty("errors")
            .EnumerateObject()
            .Select(e => e.Value[0].ToString()));
    }

    // TODO excluir trecho abaixo quando todas APIs Maxsys se tiverem versão core < 49 (ApiResultBase.MAXSYS_API)
    private static bool TitleElementStartsWith(JsonElement element, string prefix)
    {
        return element.GetString()?.StartsWith(prefix) == true;
    }

    #endregion Private

    protected override void Dispose(bool disposing)
    {
        UnsubscribeEvents();

        base.Dispose(disposing);
    }
}