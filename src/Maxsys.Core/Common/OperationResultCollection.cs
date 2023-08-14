using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Maxsys.Core;

public abstract class OperationResultCollectionBase<TOperationResult> : Collection<TOperationResult>, IOperationResult
    where TOperationResult : OperationResult
{
    #region IOperationResult

    public List<Notification>? Notifications
        => Items.Where(i => i.Notifications is not null).SelectMany(i => i.Notifications!).ToList();

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
    public bool IsValid => Items.All(i => i.IsValid);

    public virtual void SetDataToNull() { }

    #endregion IOperationResult
}

public class OperationResultCollection : OperationResultCollectionBase<OperationResult>
{ }

public class OperationResultCollection<T> : OperationResultCollectionBase<OperationResult<T>>
{
    public override void SetDataToNull()
    {
        foreach (var item in Items)
        {
            item.SetDataToNull();
        }
    }
}