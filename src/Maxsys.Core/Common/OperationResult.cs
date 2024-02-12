using System.Diagnostics.CodeAnalysis;
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

    [JsonIgnore, MemberNotNullWhen(false, nameof(Notifications))]
    public virtual bool IsValid => !(Notifications?.Where(n => n.ResultType <= ResultTypes.Warning).Any() == true);

    public virtual List<Notification>? Notifications { get; set; }

    #region IOperationResult

    /// <summary>
    /// Corresponde ao ResultType mais severo.
    /// </summary>
    public ResultTypes ResultType => Notifications?.Count > 0 ? Notifications.Min(f => f.ResultType) : ResultTypes.Success;

    public bool ContainsNotification(string notificationMessage)
        => ContainsAnyNotification(notificationMessage);

    public bool ContainsAnyNotification(params string[] notifications)
        => ContainsNotification(notification => notifications.Contains(notification.Message));

    public bool ContainsNotification(Func<Notification, bool> predicate)
        => Notifications?.Any(predicate) == true;

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
        Notifications ??= [];

        Notifications.Add(notification);
    }

    public void AddNotifications(IEnumerable<Notification> notifications)
    {
        Notifications ??= [];

        Notifications.AddRange(notifications);
    }

    public override string? ToString()
    {
        return !IsValid ? string.Join(Environment.NewLine, Notifications!.Select(n => n.ToString())) : null;
    }

    /// <summary>
    /// Converte um <see cref="OperationResult"/> em um <see cref="OperationResult{TDestination}"/>
    /// onde <see cref="OperationResult{TDestination}.Data"/> = <paramref name="data"/>;
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="data"></param>
    public OperationResult<TDestination> Cast<TDestination>(TDestination? data)
    {
        return new OperationResult<TDestination>()
        {
            Data = data,
            Notifications = Notifications?.Count > 0 ? Notifications : null
        };
    }

    /// <summary>
    /// Converte um <see cref="OperationResult"/> em um <see cref="OperationResult{TDestination}"/>
    /// onde <see cref="OperationResult{TDestination}.Data"/> = <see langword="default"/>;
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    public OperationResult<TDestination> Cast<TDestination>() => Cast<TDestination>(default);

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

    [JsonIgnore, MemberNotNullWhen(true, nameof(Data)), MemberNotNullWhen(false, nameof(Notifications))]
    public override bool IsValid => base.IsValid;

    public override List<Notification>? Notifications
    {
        get => base.Notifications;
        set => base.Notifications = value;
    }

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
       : this(data, [notification])
    { }

    public OperationResult(Notification notification)
       : this(default, notification)
    { }

    public OperationResult(string notificationMessage, ResultTypes resultType = ResultTypes.Error)
       : this(default, notificationMessage, resultType)
    { }

    public OperationResult(T? data, string notificationMessage, ResultTypes resultType = ResultTypes.Error)
       : this(data, [new(notificationMessage, null, resultType)])
    { }

    #endregion CTOR

    #region METHODS

    // Usa-se default pois se T não for tipo referência (struct, por exemplo), data não terá valor null.
    // A não ser que T seja declarado como nulável. Ex.: OperationResult<Guid?>
    public override void SetDataToNull() => Data = default;

    public OperationResult<TDestination> Cast<TDestination>(Func<T?, TDestination?> cast)
    {
        ArgumentNullException.ThrowIfNull(nameof(cast));

        return new OperationResult<TDestination>()
        {
            Data = cast(Data),
            Notifications = Notifications?.Count > 0 ? Notifications : null
        };
    }

    #endregion METHODS
}