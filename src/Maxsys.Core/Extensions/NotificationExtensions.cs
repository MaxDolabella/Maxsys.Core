namespace Maxsys.Core;

public static class NotificationExtensions
{
    public static ResultTypes ToResultType(this IEnumerable<Notification>? notifications, ResultTypes @default = ResultTypes.Success)
    {
        return notifications?.Any() == true ? notifications.Min(f => f.ResultType) : @default;
    }
}