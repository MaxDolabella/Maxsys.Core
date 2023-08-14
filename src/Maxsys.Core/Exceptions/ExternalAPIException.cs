using System.Net;

namespace Maxsys.Core.Exceptions;

/// <summary>
/// Representa um erro que ocorre ao tentar chamar uma API externa (Workspace por exemplo).
/// </summary>
public sealed class ExternalAPIException : Exception
{
    private const string DEFAULT_MESSAGE = "An error has occurred while calling an external API.";

    /// <summary>
    /// Status Code da chamada à API externa.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <param name="statusCode">Status Code da chamada à API externa.</param>
    public ExternalAPIException(HttpStatusCode statusCode)
        : base(GetMessage(statusCode))
    {
        StatusCode = statusCode;
    }

    /// <param name="statusCode">Status Code da chamada à API externa.</param>
    /// <param name="reasonPhrase">Mensagem de erro da chamada à API externa.</param>
    public ExternalAPIException(HttpStatusCode statusCode, string? reasonPhrase)
      : base(GetMessage(statusCode, reasonPhrase))
    {
        StatusCode = statusCode;
    }

    /// <param name="innerException">Exception interna.</param>
    /// <param name="statusCode">Status Code da chamada à API externa.</param>
    /// <param name="reasonPhrase">Mensagem de erro da chamada à API externa.</param>
    public ExternalAPIException(Exception innerException, HttpStatusCode statusCode, string? reasonPhrase)
        : base(GetMessage(statusCode, reasonPhrase), innerException)
    {
        StatusCode = statusCode;
    }

    private static string GetMessage(HttpStatusCode statusCode, string? message = null)
    {
        return string.IsNullOrWhiteSpace(message)
            ? $"StatusCode[{(int)statusCode}]: {DEFAULT_MESSAGE}"
            : $"StatusCode[{(int)statusCode}]: {DEFAULT_MESSAGE}\n{message}";
    }
}