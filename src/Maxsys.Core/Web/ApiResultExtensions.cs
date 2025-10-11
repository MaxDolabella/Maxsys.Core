namespace Maxsys.Core.Web;

/// <summary>
/// Extensões para conversão de ApiResult para OperationResult
/// </summary>
public static class ApiResultExtensions
{
    /// <summary>
    /// Converte um ApiResult em OperationResult
    /// </summary>
    /// <param name="apiResult">O objeto ApiResult a ser convertido</param>
    /// <returns>Uma nova instância de OperationResult com as notificações do ApiResult</returns>
    /// <remarks>
    /// Este método realiza as seguintes operações:
    /// - Copia as notificações do ApiResult para o OperationResult
    /// - Adiciona uma notificação de "Item não encontrado" quando o StatusCode é 404
    /// - Retorna uma lista vazia de notificações se o ApiResult não contiver nenhuma
    /// </remarks>
    /// <example>
    /// <code>
    /// var apiResult = new ApiResult { StatusCode = 200, Notifications = notifications };
    /// var operationResult = apiResult.ToOperationResult();
    /// </code>
    /// </example>
    public static OperationResult ToOperationResult(this ApiResult apiResult)
    {
        var o = new OperationResult(apiResult.Notifications?.ToList() ?? []);
        if (apiResult.StatusCode == 404)
        {
            o.AddNotification(new(GenericMessages.ITEM_NOT_FOUND));
        }

        return o;
    }

    /// <summary>
    /// Converte um ApiResult&lt;T&gt; em OperationResult&lt;T&gt;
    /// </summary>
    /// <typeparam name="T">O tipo dos dados contidos no resultado</typeparam>
    /// <param name="apiResult">O objeto ApiResult&lt;T&gt; a ser convertido</param>
    /// <returns>Uma nova instância de OperationResult&lt;T&gt; com os dados e notificações do ApiResult</returns>
    /// <remarks>
    /// Este método realiza as seguintes operações:
    /// - Preserva os dados (Data) do ApiResult original
    /// - Copia as notificações do ApiResult para o OperationResult
    /// - Adiciona uma notificação de "Item não encontrado" quando o StatusCode é 404
    /// - Retorna uma lista vazia de notificações se o ApiResult não contiver nenhuma
    /// </remarks>
    /// <example>
    /// <code>
    /// var apiResult = new ApiResult&lt;User&gt; { Data = user, StatusCode = 200, Notifications = notifications };
    /// var operationResult = apiResult.ToOperationResult();
    /// </code>
    /// </example>
    public static OperationResult<T> ToOperationResult<T>(this ApiResult<T> apiResult)
    {
        var o = new OperationResult<T>(apiResult.Data, apiResult.Notifications?.ToList() ?? []);
        if (apiResult.StatusCode == 404)
        {
            o.AddNotification(new(GenericMessages.ITEM_NOT_FOUND));
        }

        return o;
    }

    /// <summary>
    /// Converte um ApiResult em OperationResult&lt;T&gt; com dados padrão
    /// </summary>
    /// <typeparam name="T">O tipo dos dados do OperationResult resultante</typeparam>
    /// <param name="apiResult">O objeto ApiResult a ser convertido</param>
    /// <returns>Uma nova instância de OperationResult&lt;T&gt; com valor padrão para dados e notificações do ApiResult</returns>
    /// <remarks>
    /// Este método realiza as seguintes operações:
    /// - Define os dados como valor padrão (default) do tipo T
    /// - Copia as notificações do ApiResult para o OperationResult
    /// - Adiciona uma notificação de "Item não encontrado" quando o StatusCode é 404
    /// - Retorna uma lista vazia de notificações se o ApiResult não contiver nenhuma
    /// - Útil quando você precisa converter um ApiResult sem dados tipados para um OperationResult tipado
    /// </remarks>
    /// <example>
    /// <code>
    /// var apiResult = new ApiResult { StatusCode = 404, Notifications = notifications };
    /// var operationResult = apiResult.ToOperationResult&lt;User&gt;();
    /// // operationResult.Data será null (valor padrão para User)
    /// </code>
    /// </example>
    public static OperationResult<T> ToOperationResult<T>(this ApiResult apiResult)
    {
        var o = new OperationResult<T>(default, apiResult.Notifications?.ToList() ?? []);
        if (apiResult.StatusCode == 404)
        {
            o.AddNotification(new(GenericMessages.ITEM_NOT_FOUND));
        }

        return o;
    }
}