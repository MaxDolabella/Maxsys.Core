using System.Text.Json.Serialization;

namespace Maxsys.Core.Web;

public sealed class ResultItem<T>
{
    [JsonPropertyOrder(1)]
    public T? Data { get; set; } = default;

    [JsonPropertyOrder(2)]
    public IEnumerable<Notification>? Notifications { get; set; }

    /// <summary>
    /// Corresponde ao ResultType mais severo.
    /// </summary>
    [JsonPropertyOrder(0)]
    public ResultTypes ResultType => Notifications?.Any() == true ? Notifications.Min(f => f.ResultType) : ResultTypes.Success;

    #region CTOR

    public ResultItem(T? data, IEnumerable<Notification>? notifications)
    {
        Data = data;
        Notifications = notifications;
    }

    public ResultItem() : this(default, default)
    { }

    #endregion CTOR
}