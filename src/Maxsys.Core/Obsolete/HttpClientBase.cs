using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;

namespace Maxsys.Core.Services.Http;

// remover em v18
[Obsolete("This service war discontinued and will be removed. Use HttpServiceBase (or MaxsysHttpServiceBase for Maxsys APIs).", true)]
public abstract class HttpClientBase : ServiceBase
{
    protected HttpClientBase(ILogger _0, IHttpClientFactory _1)
    { }

    protected HttpClientBase(ILogger _0, string _1, IHttpClientFactory _2)
    { }

    protected abstract ValueTask<AuthenticationHeaderValue?> AddAuthTokenAsync(CancellationToken cancellationToken = default);

    protected virtual ValueTask<HttpRequestMessage> GetHttpRequestMessageAsync(HttpMethod requestMethod, string requestUri, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    protected virtual ValueTask<HttpRequestMessage> GetHttpRequestMessageWithoutAuthenticationAsync(HttpMethod requestMethod, string requestUri, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    protected virtual ValueTask<HttpRequestMessage> GetHttpRequestMessageAsync<TJsonContent>(HttpMethod requestMethod, string requestUri, TJsonContent requestJsonContent, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    protected virtual ValueTask<HttpRequestMessage> GetHttpRequestMessageWithContentAsync(HttpMethod requestMethod, string requestUri, HttpContent requestContent, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    protected virtual Task<(HttpResponseMessage Message, string Content)> GetHttpResponseMessageAsync(HttpMethod requestMethod, string requestUri, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    protected virtual Task<(HttpResponseMessage Message, string Content)> GetHttpResponseMessageAsync<TJsonContent>(HttpMethod requestMethod, string requestUri, TJsonContent requestJsonContent, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    protected virtual Task<(HttpResponseMessage Message, string Content)> GetHttpResponseMessageAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    protected virtual Task<(HttpResponseMessage Message, string Content)> GetApiResponseAsync(HttpMethod requestMethod, string requestUri, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    protected virtual Task<(HttpResponseMessage Message, string Content)> GetApiResponseAsync<TContent>(HttpMethod requestMethod, string requestUri, TContent requestContent, IDictionary<string, string>? requestHeaders, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    protected virtual void ThrowExternalAPIExceptionIfResponseIsNoApiResultWithTitlePrefix((HttpResponseMessage Message, string Content) response)
        => throw new NotImplementedException();
}