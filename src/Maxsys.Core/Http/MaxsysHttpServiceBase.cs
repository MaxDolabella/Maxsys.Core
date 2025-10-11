using System.Net;
using System.Text.Json;
using Maxsys.Core.Extensions;
using Maxsys.Core.Web;

namespace Maxsys.Core.Http;

public abstract class MaxsysHttpServiceBase : HttpServiceBase
{
    protected MaxsysHttpServiceBase(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    { }

    #region Events

    // Handlers

    public event MaxsysApiResponseInvalidEventHandler? MaxsysApiResponseInvalid;

    public event MaxsysApiResponseValidEventHandler? MaxsysApiResponseValid;

    // Hooks
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

    protected override void UnsubscribeEvents()
    {
        MaxsysApiResponseInvalid = null;
        MaxsysApiResponseValid = null;

        base.UnsubscribeEvents();
    }

    #endregion Events

    #region Helpers

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
            : new OperationResult<T>(notification);
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
            : new OperationResult(notification);
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

            return root.TryGetProperty(ApiResultBase.MAXSYS_API, out var _)
                ? MaxsysApiValidationResult.CreateValidResult()
                : MaxsysApiValidationResult.CreateInvalidResult(responseMessage.StatusCode, "Maxsys API validation fails: expected identifier not found.", responseContent);
        }
        catch (Exception ex)
        {
            return MaxsysApiValidationResult.CreateInvalidResult(responseMessage.StatusCode, "Error", responseContent, ex);
        }
    }

    #endregion Private
}