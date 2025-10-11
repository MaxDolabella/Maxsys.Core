using System.Net.Http.Json;
using System.Text.Json;
using Maxsys.Core.Extensions;
using Maxsys.Core.Services;

namespace Maxsys.Core.Http;

public abstract class HttpServiceBase : ServiceBase
{
    protected readonly HttpClient _httpClient;

    protected HttpServiceBase(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    #region Events

    // Handlers

    public event SendingEventHandler? Sending;

    public event SentEventHandler? Sent;

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

    protected virtual void UnsubscribeEvents()
    {
        Sending = null;
        Sent = null;
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

    protected static string GetErrorsFromPropertyInJsonElement(JsonElement element)
    {
        return string.Join('\n', element
            .GetProperty("errors")
            .EnumerateObject()
            .Select(e => e.Value[0].ToString()));
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

    protected override void Dispose(bool disposing)
    {
        UnsubscribeEvents();

        base.Dispose(disposing);
    }
}