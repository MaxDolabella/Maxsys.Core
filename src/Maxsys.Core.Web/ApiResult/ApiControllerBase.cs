using Maxsys.Core.Web.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maxsys.Core.Web;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected virtual Func<IOperationResult, int>? DefaultNotOkStatusCodeFunc { get; set; } = null;

    /// <summary>
    /// Retorna um <see cref="ApiResult{T}"/> com um <see cref="ApiResult{T}.Data">ApiResult.Data</see> do tipo <typeparamref name="T"/>.<br/>
    /// Normalmente utilizado para retornar um objeto (nulo ou não) e/ou uma lista de <see cref="Notification"/>.
    /// </summary>
    /// <typeparam name="T">tipo da property 'data'.</typeparam>
    /// <param name="data">contém um dado (ou nulo) do tipo <typeparamref name="T"/>.</param>
    /// <param name="okStatus">Opcional. Se passado e a property <paramref name="data"/> for diferente de <see langword="null"/>,
    ///     esse será o valor do status code.<br/>
    ///     Padrão = 200 Ok.
    /// </param>
    /// <param name="nullDataStatusCode">Opcional. Se passado e a property <paramref name="data"/> for igual a <see langword="null"/>,
    ///     esse será o valor do status code.<br/>
    ///     Padrão = 404 Not Found.
    /// </param>
    /// <param name="resultType">Opcional. Contém o tipo do resultado (severidade de erro/sucesso) da requisição.<br/>
    ///     Padrão é <see cref="ResultTypes.Success"/>.
    /// </param>
    [NonAction]
    protected ApiActionResult ApiDataResult<T>(T? data, int okStatus = StatusCodes.Status200OK, int nullDataStatusCode = StatusCodes.Status404NotFound, ResultTypes? resultType = null)
    {
        var endpointIdentifier = HttpContext.GetEndpointIdentifier();
        var statusCode = data is not null ? okStatus : nullDataStatusCode;

        var resultType2 = resultType ?? StatusCodeToResultType(statusCode);

        var apiResult = new ApiResult<T?>(endpointIdentifier, statusCode, resultType2, data);

        return new ApiActionResult(apiResult);
    }

    /// <summary>
    /// Retorna um <see cref="ApiResult{X}"/> com um <see cref="ApiResult{X}.Data">ApiResult.Data</see> do tipo <see cref="ListDTO{T}"/>.<br/>
    /// </summary>
    /// <typeparam name="T">tipo da property 'Data'.</typeparam>
    /// <param name="list">contém um um <see cref="ListDTO{T}"/>.</param>
    /// <param name="resultType">Opcional. Contém o tipo do resultado (severidade de erro/sucesso) da requisição. Padrão é <see cref="ResultTypes.Success"/>.</param>
    /// <returns></returns>
    [NonAction]
    protected ApiActionResult ApiListResult<T>(ListDTO<T> list, ResultTypes resultType = ResultTypes.Success)
    {
        var endpointIdentifier = HttpContext.GetEndpointIdentifier();
        var apiResult = new ApiResult<ListDTO<T>>(endpointIdentifier, StatusCodes.Status200OK, resultType, list);

        return new ApiActionResult(apiResult);
    }

    /// <summary>
    /// Retorna um <see cref="ApiResult"/> que não contém 'Data' e pode conter uma lista de <see cref="Notification"/>.<br/>
    /// Normalmente utilizado para retornar o resultado de uma operação.
    /// </summary>
    /// <param name="operationResult">o <see cref="OperationResult"/> de onde será verificada a validade da operação e seus possíveis erros/notificações.</param>
    /// <param name="okStatus">
    ///     Opcional. Se passado e <paramref name="operationResult"/>.IsValid == <see langword="true"/>, esse será o valor do status code.<br/>
    ///     Padrão = 200 Ok.
    /// </param>
    /// <param name="toNotOkStatusCodeFunc">
    ///     Opcional. Se passado e <paramref name="operationResult"/>.IsValid == <see langword="false"/>, o status code será o valor retornado por essa <see cref="Func{T, TResult}"/>. <br/>
    ///     Caso não seja passado, o status code será o valor retornado pela Func <see cref="DefaultNotOkStatusCodeFunc">DefaultNotOkStatusCodeFunc</see>.<br/>
    ///     Caso <see cref="DefaultNotOkStatusCodeFunc">DefaultNotOkStatusCodeFunc</see> seja igual a <see langword="null"/>, o status code será 400 Bad Request.<br/>
    ///     Padrão = <see langword="null"/>
    /// </param>
    [NonAction]
    protected ApiActionResult ApiOperationResult(OperationResult operationResult, int okStatus = StatusCodes.Status200OK, Func<IOperationResult, int>? toNotOkStatusCodeFunc = null)
    {
        var endpointIdentifier = HttpContext.GetEndpointIdentifier();
        var status = operationResult.IsValid
            ? okStatus
            : toNotOkStatusCodeFunc?.Invoke(operationResult)
                ?? DefaultNotOkStatusCodeFunc?.Invoke(operationResult)
                ?? StatusCodes.Status400BadRequest;

        var apiResult = new ApiResult(endpointIdentifier, status, operationResult);

        return new ApiActionResult(apiResult);
    }

    /// <summary>
    /// Retorna um <see cref="ApiResult{T}"/> com um <see cref="ApiResult{T}.Data">ApiResult.Data</see> do tipo <typeparamref name="T"/>.<br/>
    /// Normalmente utilizado para retornar o resultado de uma operação e pode conter uma lista de <see cref="Notification"/>.
    /// </summary>
    /// <typeparam name="T">tipo da property 'Data'.</typeparam>
    /// <param name="operationResult">o <see cref="OperationResult"/> de onde será verificada a validade da operação e seus possíveis erros/notificações.</param>
    /// <param name="okStatus">
    ///     Opcional. Se passado e <paramref name="operationResult"/>.IsValid == <see langword="true"/>, esse será o valor do status code.<br/>
    ///     Padrão = 200 Ok.
    /// </param>
    /// <param name="toNotOkStatusCodeFunc">
    ///     Opcional. Se passado e <paramref name="operationResult"/>.IsValid == <see langword="false"/>, o status code será o valor retornado por essa <see cref="Func{T, TResult}"/>. <br/>
    ///     Caso não seja passado, o status code será o valor retornado pela Func <see cref="DefaultNotOkStatusCodeFunc">DefaultNotOkStatusCodeFunc</see>.<br/>
    ///     Caso <see cref="DefaultNotOkStatusCodeFunc">DefaultNotOkStatusCodeFunc</see> seja igual a <see langword="null"/>, o status code será 400 Bad Request.<br/>
    ///     Padrão = <see langword="null"/>
    /// </param>
    /// <param name="returnData">
    ///     Opcional. Indica se <see cref="ApiResult{T}.Data"/> deve ser exibido.
    ///     Padrão = <see langword="true"/>.
    /// </param>
    [NonAction]
    protected ApiActionResult ApiOperationResult<T>(OperationResult<T> operationResult, int okStatus = StatusCodes.Status200OK, Func<IOperationResult, int>? toNotOkStatusCodeFunc = null, bool returnData = true)
    {
        var endpointIdentifier = HttpContext.GetEndpointIdentifier();
        var status = operationResult.IsValid
            ? okStatus
            : toNotOkStatusCodeFunc?.Invoke(operationResult)
                ?? DefaultNotOkStatusCodeFunc?.Invoke(operationResult)
                ?? StatusCodes.Status400BadRequest;

        if (!returnData)
            operationResult.SetDataToNull();

        var apiResult = new ApiResult<T>(endpointIdentifier, status, operationResult);

        return new ApiActionResult(apiResult);
    }

    /// <summary>
    /// Retorna um <see cref="ApiMultipleResults{T}"/> com um <see cref="ApiResult{X}.Data">ApiMultipleResults.Data</see>
    /// do tipo <see cref="IEnumerable{X}"/> e T do tipo <see cref="ResultItem{T}"/>.<br/>
    /// Normalmente utilizado para retornar o resultado de múltiplas operações que podem conter um dado e uma lista de <see cref="Notification"/> cada uma.
    /// </summary>
    /// <typeparam name="T">tipo da property 'Data'.</typeparam>
    /// <param name="operationResult">o <see cref="OperationResult"/> de onde será verificada a validade da operação e seus possíveis erros/notificações.</param>
    /// <param name="okStatus">
    ///     Opcional. Se passado e <paramref name="operationResult"/>.IsValid == <see langword="true"/>, esse será o valor do status code.<br/>
    ///     Padrão = 200 Ok.
    /// </param>
    /// <param name="toNotOkStatusCodeFunc">
    ///     Opcional. Se passado e <paramref name="operationResult"/>.IsValid == <see langword="false"/>, o status code será o valor retornado por essa <see cref="Func{T, TResult}"/>. <br/>
    ///     Caso não seja passado, o status code será o valor retornado pela Func <see cref="DefaultNotOkStatusCodeFunc">DefaultNotOkStatusCodeFunc</see>.<br/>
    ///     Caso <see cref="DefaultNotOkStatusCodeFunc">DefaultNotOkStatusCodeFunc</see> seja igual a <see langword="null"/>, o status code será 400 Bad Request.<br/>
    ///     Padrão = <see langword="null"/>
    /// </param>
    /// <param name="returnData">
    ///     Opcional. Indica se cada <see cref="ResultItem{T}.Data"/> deve ser exibido.
    ///     Padrão = <see langword="true"/>.
    /// </param>
    [NonAction]
    protected ApiActionResult ApiOperationResult<T>(OperationResultCollection<T> operationResult, int okStatus = StatusCodes.Status200OK, Func<IOperationResult, int>? toNotOkStatusCodeFunc = null, bool returnData = true)
    {
        var endpointIdentifier = HttpContext.GetEndpointIdentifier();
        var status = operationResult.IsValid
            ? okStatus
            : toNotOkStatusCodeFunc?.Invoke(operationResult)
                ?? DefaultNotOkStatusCodeFunc?.Invoke(operationResult)
                ?? StatusCodes.Status400BadRequest;

        if (!returnData)
            operationResult.SetDataToNull();

        var apiResult = new ApiMultipleResults<T>(endpointIdentifier, status, operationResult);

        return new ApiActionResult(apiResult);
    }

    /// <summary>
    /// Retorna um <see cref="ApiResult"/> que não contém 'Data' e pode conter uma lista de <see cref="Notification"/>.<br/>
    /// Normalmente utilizado para retornar o resultado de múltiplas operações.
    /// </summary>
    /// <param name="operationResult">o <see cref="OperationResult"/> de onde será verificada a validade da operação e seus possíveis erros/notificações.</param>
    /// <param name="okStatus">
    ///     Opcional. Se passado e <paramref name="operationResult"/>.IsValid == <see langword="true"/>, esse será o valor do status code.<br/>
    ///     Padrão = 200 Ok.
    /// </param>
    /// <param name="toNotOkStatusCodeFunc">
    ///     Opcional. Se passado e <paramref name="operationResult"/>.IsValid == <see langword="false"/>, o status code será o valor retornado por essa <see cref="Func{T, TResult}"/>. <br/>
    ///     Caso não seja passado, o status code será o valor retornado pela Func <see cref="DefaultNotOkStatusCodeFunc">DefaultNotOkStatusCodeFunc</see>.<br/>
    ///     Caso <see cref="DefaultNotOkStatusCodeFunc">DefaultNotOkStatusCodeFunc</see> seja igual a <see langword="null"/>, o status code será 400 Bad Request.<br/>
    ///     Padrão = <see langword="null"/>
    /// </param>
    [NonAction]
    protected ApiActionResult ApiOperationResult(OperationResultCollection operationResult, int okStatus = StatusCodes.Status200OK, Func<IOperationResult, int>? toNotOkStatusCodeFunc = null)
    {
        var endpointIdentifier = HttpContext.GetEndpointIdentifier();
        var status = operationResult.IsValid
            ? okStatus
            : toNotOkStatusCodeFunc?.Invoke(operationResult)
                ?? DefaultNotOkStatusCodeFunc?.Invoke(operationResult)
                ?? StatusCodes.Status400BadRequest;

        var notifications = operationResult.Notifications?.Count > 0
            ? operationResult.Notifications!
            : null;

        var apiResult = new ApiResult(endpointIdentifier, status, operationResult.ResultType, notifications);

        return new ApiActionResult(apiResult);
    }

    /// <summary>
    /// <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Status"/>
    /// </summary>
    /// <param name="statusCode"></param>
    [NonAction]
    private static ResultTypes StatusCodeToResultType(int statusCode)
    {
        return statusCode switch
        {
            >= 100 and <= 199 => ResultTypes.Info,
            >= 200 and <= 399 => ResultTypes.Success,
            >= 400 and <= 499 => ResultTypes.Warning,
            _ => ResultTypes.Error,
        };
    }

    #region Obsolete

    [NonAction]
    [Obsolete("Utilizar novo ApiDataResult", true)]
    protected ApiActionResult CustomDataResult<T>(string endpointTitle, T? data, int okStatus = StatusCodes.Status200OK, int nullDataStatusCode = StatusCodes.Status404NotFound, ResultTypes? resultType = null)
        => throw new NotImplementedException();

    [NonAction]
    [Obsolete("Utilizar novo ApiListResult", true)]
    protected ApiActionResult CustomListResult<T>(string endpointTitle, ListDTO<T> list, ResultTypes resultType = ResultTypes.Success)
        => throw new NotImplementedException();

    [NonAction]
    [Obsolete("Utilizar novo ApiOperationResult", true)]
    protected ApiActionResult CustomOperationResult(string endpointTitle, OperationResult operationResult, int okStatus = StatusCodes.Status200OK, Func<IOperationResult, int>? toNotOkStatusCodeFunc = null)
        => throw new NotImplementedException();

    [NonAction]
    [Obsolete("Utilizar novo ApiOperationResult", true)]
    protected ApiActionResult CustomOperationResult<T>(string endpointTitle, OperationResult<T> operationResult, int okStatus = StatusCodes.Status200OK, Func<IOperationResult, int>? toNotOkStatusCodeFunc = null, bool returnData = true)
        => throw new NotImplementedException();

    [NonAction]
    [Obsolete("Utilizar novo ApiOperationResult", true)]
    protected ApiActionResult CustomOperationResult<T>(string endpointTitle, OperationResultCollection<T> operationResult, int okStatus = StatusCodes.Status200OK, Func<IOperationResult, int>? toNotOkStatusCodeFunc = null, bool returnData = true)
        => throw new NotImplementedException();

    [NonAction]
    [Obsolete("Utilizar novo ApiOperationResult", true)]
    protected ApiActionResult CustomOperationResult(string endpointTitle, OperationResultCollection operationResult, int okStatus = StatusCodes.Status200OK, Func<IOperationResult, int>? toNotOkStatusCodeFunc = null)
        => throw new NotImplementedException();

    #endregion Obsolete
}