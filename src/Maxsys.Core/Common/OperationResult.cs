using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Maxsys.Core;

/// <summary>
/// Representa o resultado de uma operação podendo conter
/// Notificações referentes ao resultado da operação.
/// </summary>
public partial class OperationResult : IOperationResult
{
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
        : this([notification])
    { }

    #endregion CTOR

    #region METHODS

    // Método privado para evitar repetição de código ao adicionar notificações. (Notifications ??= [];)
    private void InternalAddNotification(Notification notification)
    {
        if (!ContainsNotification(x => x == notification))
        {
            Notifications!.Add(notification);
        }
    }

    /// <summary>
    /// Adiciona uma notificação ao resultado da operação caso ela ainda não exista.
    /// </summary>
    /// <param name="notification">A notificação a ser adicionada.</param>
    public void AddNotification(Notification notification)
    {
        Notifications ??= [];

        InternalAddNotification(notification);
    }

    /// <summary>
    /// Adiciona uma coleção de notificações ao resultado da operação caso elas ainda não existam.
    /// </summary>
    /// <param name="notifications">A coleção de notificações a ser adicionada.</param>
    public void AddNotifications(IEnumerable<Notification> notifications)
    {
        Notifications ??= [];

        foreach (var notification in notifications)
        {
            InternalAddNotification(notification);
        }
    }

    public void AddNotification(string message, string? details = null, ResultTypes resultType = ResultTypes.Error)
    {
        AddNotification(message, details, tag: null, resultType);
    }

    public void AddNotification(string message, string? details, object? tag, ResultTypes resultType = ResultTypes.Error)
    {
        AddNotification(new Notification(message, details, resultType)
        {
            Tag = tag
        });
    }

    public void AddException(Exception exception, string? customMessage = null)
    {
        Notification notification = string.IsNullOrWhiteSpace(customMessage)
            ? new(exception)
            : new(exception, customMessage);

        AddNotification(notification);
    }

    public void AddWarningNotification(string message, string? details = null, object? tag = null)
    {
        AddNotification(message, details, tag, ResultTypes.Warning);
    }

    public void AddErrorNotification(string message, string? details = null, object? tag = null)
    {
        AddNotification(message, details, tag, ResultTypes.Error);
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
public partial class OperationResult<T> : OperationResult
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
    public OperationResult() : base()
    { }

    public OperationResult(T? data, List<Notification>? notifications)
       : this()
    {
        Data = data;
        Notifications = notifications;
    }

    public OperationResult(List<Notification> notifications)
        : this(default, notifications)
    { }

    public OperationResult(Notification notification)
        : this(default, [notification])
    { }

    public OperationResult(T? data, Notification notification)
       : this(data, [notification])
    { }

    #endregion CTOR

    #region METHODS

    // Usa-se default pois se T não for tipo referência (struct, por exemplo), data não terá valor null.
    // A não ser que T seja declarado como nulável. Ex.: OperationResult<Guid?>
    public override void SetDataToNull() => Data = default;

    public OperationResult<TDestination> Cast<TDestination>(Func<T?, TDestination?> cast)
    {
        ArgumentNullException.ThrowIfNull(cast, nameof(cast));

        return new OperationResult<TDestination>()
        {
            Data = cast(Data),
            Notifications = Notifications?.Count > 0 ? Notifications : null
        };
    }

    #endregion METHODS
}

public static class OperationResultExtensions
{
    extension(OperationResult result)
    {
        public OperationResult<T> WithData<T>(T? data) => result.Cast(data);
    }

    extension<T>(OperationResult<T> result)
    {
        public OperationResult<T> WithData(T? data)
        {
            result.Data = data;
            return result;
        }
    }
}