namespace Maxsys.Core;

/// <summary>
/// Representa uma notificação em uma operação (<see cref="OperationResult"/>).
/// </summary>
public sealed class Notification
{
    #region CTORs

    /// <summary>
    /// CTOR vazio necessário para conversão de Json
    /// </summary>
    public Notification()
    { }

    /// <summary>
    /// CTOR full
    /// </summary>
    /// <param name="message"></param>
    /// <param name="details"></param>
    /// <param name="resultType"></param>
    /// <param name="tag"></param>
    public Notification(string message, string? details, ResultTypes resultType, object? tag)
    {
        Message = message;
        Details = details;
        ResultType = resultType;
        Tag = tag;
    }

    public Notification(string message, ResultTypes resultType = ResultTypes.Error)
        : this(message, null, resultType, null)
    { }

    public Notification(string message, string? details, ResultTypes resultType = ResultTypes.Error)
        : this(message, details, resultType, null)
    { }

    public Notification(Exception exception, string message, ResultTypes resultType = ResultTypes.Error)
        : this(message, exception.InnerException?.Message, resultType, exception.ToString())
    { }

    public Notification(Exception exception, ResultTypes resultType = ResultTypes.Error)
        : this(exception, exception.Message, resultType)
    { }

    #endregion CTORs

    #region PROPS

    /// <summary>
    /// Severidade do erro
    /// </summary>
    public ResultTypes ResultType { get; init; }

    /// <summary>
    /// Mensagem do erro.
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    /// Mensagem complementar ao erro ou exception
    /// </summary>
    public string? Details { get; init; }

    /// <summary>
    /// Um <see langword="object"/> que contém um dado. O valor padrão é <see langword="null"/>.
    /// </summary>
    public object? Tag { get; set; }

    #endregion PROPS

    public override string ToString()
    {
        var message = !string.IsNullOrWhiteSpace(Details)
            ? $"{Message} [{Details}]"
            : Message;
        return $"{ResultType}: {message}";
    }
}