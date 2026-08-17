using System.Text;

namespace Maxsys.Core;

/// <summary>
/// Representa uma notificação em uma operação (<see cref="OperationResult"/>).
/// </summary>
public sealed class Notification : IEquatable<Notification>
{
    #region CTORs

    /// <summary>
    /// CTOR vazio necessário para conversão de Json
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public Notification()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    { }

    public Notification(string message, ResultTypes resultType = ResultTypes.Error)
        : this(message, null, resultType)
    { }

    public Notification(string message, string? details, ResultTypes resultType = ResultTypes.Error)
    {
        Message = message;
        Details = details;
        ResultType = resultType;
    }

    public Notification(Exception exception, ResultTypes resultType = ResultTypes.Error)
        : this(exception, exception.Message, resultType)
    { }

    public Notification(Exception exception, string message, ResultTypes resultType = ResultTypes.Error)
    {
        var detailsBuilder = new StringBuilder();
        if (exception.Message != message)
        {
            detailsBuilder.AppendLine(exception.Message);
        }
        if (exception.InnerException is not null)
        {
            // TODO método recursivo?
            detailsBuilder.AppendLine(exception.InnerException.Message);
        }

        Message = message;
        Details = detailsBuilder.Length == 0 ? null : detailsBuilder.ToString();
        ResultType = resultType;

#if DEBUG
        Tag = exception.ToString();
#endif
    }

    #endregion CTORs

    #region Props

    /// <summary>
    /// Severidade do erro
    /// </summary>
    public ResultTypes ResultType { get; init; }

    /// <summary>
    /// Código do erro.
    /// </summary>
    /// <remarks><example>Ex.:ITEM_NOT_FOUND</example></remarks>
    public string Message { get; init; }

    /// <summary>
    /// Mensagem complementar ao erro ou exception
    /// </summary>
    public string? Details { get; init; }

    /// <summary>
    /// Um <see langword="object"/> que contém um dado. O valor padrão é <see langword="null"/>.
    /// </summary>
    public object? Tag { get; set; }

    #endregion Props

    #region Methods

    /// <summary>
    /// Custom ToString()
    /// </summary>
    public override string ToString()
    {
        return !string.IsNullOrWhiteSpace(Details)
            ? $"{Message} [{Details}]"
            : Message;
    }

    #region Equals, == operator

    public bool Equals(Notification? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return ResultType == other.ResultType
            && Message == other.Message
            && Details == other.Details
            && Equals(Tag, other.Tag);
    }

    public override bool Equals(object? obj) => Equals(obj as Notification);

    public override int GetHashCode() => HashCode.Combine(ResultType, Message, Details, Tag);

    public static bool operator ==(Notification? left, Notification? right)
        => left is null ? right is null : left.Equals(right);

    public static bool operator !=(Notification? left, Notification? right)
        => !(left == right);

    #endregion Equals, == operator

    #endregion Methods
}