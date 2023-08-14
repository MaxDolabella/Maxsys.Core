using System.Text.Json.Serialization;
using FluentValidation.Results;

namespace Maxsys.Core;

/// <summary>
/// Representa o resultado de uma operação podendo conter
/// Notificações referentes ao resultado da operação.
/// </summary>
public class OperationResult : IOperationResult
{
    public static readonly OperationResult Empty = new();

    public List<Notification>? Notifications { get; set; }

    #region IOperationResult

    /// <summary>
    /// Corresponde ao ResultType mais severo.
    /// </summary>
    public ResultTypes ResultType => IsValid
        ? ResultTypes.Success
        : Notifications!.Min(f => f.ResultType);

    public bool ContainsNotification(string notificationMessage)
        => ContainsAnyNotification(notificationMessage);

    public bool ContainsAnyNotification(params string[] notifications)
        => ContainsNotification(notification => notifications.Contains(notification.Message));

    public bool ContainsNotification(Func<Notification, bool> predicate)
        => Notifications?.Any(predicate) == true;

    [JsonIgnore]
    public bool IsValid => !(Notifications?.Where(n => n.ResultType <= ResultTypes.Warning).Any() == true);

    public virtual void SetDataToNull()
    { /* Faz nada pois não tem data. */ }

    #endregion IOperationResult

    #region CTOR

    public OperationResult()
    {
        Notifications = null;
    }

    public OperationResult(List<Notification> notifications)
    {
        Notifications = notifications;
    }

    public OperationResult(Notification notification)
        : this(new List<Notification> { notification })
    { }

    public OperationResult(string notificationMessage)
        : this(new Notification(notificationMessage))
    { }

    public OperationResult(string notificationMessage, string notificationDetails)
        : this(new Notification(notificationMessage, notificationDetails))
    { }

    public OperationResult(ValidationResult validationResult)
    {
        Notifications = validationResult.ToNotifications();
    }

    public OperationResult(Exception exception, ResultTypes resultType = ResultTypes.Error)
    {
        var failures = new List<Notification>();

        if (exception.InnerException is not null)
        {
            failures.Add(new Notification(
                exception.InnerException.Message,
                exception.InnerException.ToString(),
                resultType));
        }

        failures.Add(new Notification(
                exception.Message,
                exception.ToString(),
                resultType));

        Notifications = failures;
    }

    #endregion CTOR

    #region METHODS

    public void AddNotification(Notification notification)
    {
        Notifications ??= new List<Notification>();

        Notifications.Add(notification);
    }

    public void AddNotifications(IEnumerable<Notification> notifications)
    {
        Notifications ??= new List<Notification>();

        Notifications.AddRange(notifications);
    }

    public override string? ToString()
    {
        return !IsValid ? string.Join(Environment.NewLine, Notifications!.Select(n => n.ToString())) : null;
    }

    #endregion METHODS
}

/// <summary>
/// Representa o resultado de uma operação podendo conter
/// um objeto do tipo <typeparamref name="T"/> e Notificações
/// referentes ao resultado da operação.
/// </summary>
/// <typeparam name="T"></typeparam>
public class OperationResult<T> : OperationResult
{
    #region PROPS

    public T? Data { get; set; }

    #endregion PROPS

    #region CTOR

    /// <summary>
    /// CTOR vazio necessário para conversão de Json
    /// </summary>
    public OperationResult()
    { }

    public OperationResult(T? data, List<Notification>? notifications)
       : base()
    {
        Data = data;
        Notifications = notifications;
    }

    public OperationResult(List<Notification>? notifications)
       : this(default, notifications)
    { }

    public OperationResult(T? data, ValidationResult validationResult)
       : this(data, validationResult.ToNotifications())
    { }

    public OperationResult(ValidationResult validationResult)
        : this(default, validationResult.ToNotifications())
    { }

    public OperationResult(T data)
        : this(data, default(List<Notification>?))
    { }

    public OperationResult(Exception exception, ResultTypes resultType = ResultTypes.Error)
        : this(default, exception, resultType)
    { }

    public OperationResult(T? data, Exception exception, ResultTypes resultType = ResultTypes.Error)
        : base(exception, resultType)
    {
        Data = data;
    }

    public OperationResult(T? data, Notification notification)
       : this(data, new List<Notification> { notification })
    { }

    public OperationResult(Notification notification)
       : this(default, notification)
    { }

    public OperationResult(string notificationMessage, ResultTypes resultType = ResultTypes.Error)
       : this(default, notificationMessage, resultType)
    { }

    public OperationResult(T? data, string notificationMessage, ResultTypes resultType = ResultTypes.Error)
       : this(data, new List<Notification> { new(notificationMessage, null, resultType) })
    { }

    #endregion CTOR

    #region METHODS

    // Usa-se default pois se T não for tipo referência (struct, por exemplo), data não terá valor null.
    // A não ser que T seja declarado como nulável. Ex.: OperationResult<Guid?>
    public override void SetDataToNull() => Data = default;

    #endregion METHODS
}