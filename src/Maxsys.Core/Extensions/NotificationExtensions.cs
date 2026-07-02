namespace Maxsys.Core;

public static class NotificationExtensions
{
    extension(IEnumerable<Notification>? notifications)
    {
        public ResultTypes ToResultType(ResultTypes @default = ResultTypes.Success)
        {
            return notifications?.Any() == true ? notifications.Min(f => f.ResultType) : @default;
        }
    }
}