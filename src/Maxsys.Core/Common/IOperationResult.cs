using System.Text.Json.Serialization;

namespace Maxsys.Core;

public interface IOperationResult
{
    ResultTypes ResultType { get; }

    [JsonIgnore]
    bool IsValid { get; }

    List<Notification>? Notifications { get; }

    bool ContainsNotification(Func<Notification, bool> predicate);

    bool ContainsNotification(string notification);

    bool ContainsAnyNotification(params string[] notifications);

    void SetDataToNull();
}