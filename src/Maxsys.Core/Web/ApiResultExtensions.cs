namespace Maxsys.Core.Web;

public static class ApiResultExtensions
{
    public static OperationResult ToOperationResult(this ApiResult apiResult)
    {
        return new OperationResult()
        {
            Notifications = apiResult.Notifications?.ToList()
        };
    }

    public static OperationResult<T> ToOperationResult<T>(this ApiResult<T> apiResult)
    {
        return new OperationResult<T>()
        {
            Notifications = apiResult.Notifications?.ToList(),
            Data = apiResult.Data
        };
    }
}