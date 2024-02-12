namespace Maxsys.Core.Web;

public class ApiResult : ApiResultBase
{
    public IEnumerable<Notification>? Notifications { get; set; }

    #region CTOR

    public ApiResult() : base(string.Empty, 200, ResultTypes.Success)
    { }

    public ApiResult(string title, int statusCode, ResultTypes resultType, IEnumerable<Notification>? notifications)
        : base(title, statusCode, notifications.ToResultType(resultType))
    {
        Notifications = notifications;
    }

    public ApiResult(string title, int statusCode, OperationResult operationResult)
        : this(title, statusCode, operationResult.ResultType, operationResult.Notifications)
    { }

    #endregion CTOR
}

public class ApiResult<T> : ApiResult
{
    public T? Data { get; set; } = default;

    #region CTOR

    public ApiResult() : base()
    { }

    public ApiResult(string title, int statusCode, ResultTypes resultType, T? data, IEnumerable<Notification>? notifications)
        : base(title, statusCode, resultType, notifications)
    {
        Data = data;
    }

    public ApiResult(string title, int statusCode, ResultTypes resultType, T data)
        : this(title, statusCode, resultType, data, null)
    { }

    public ApiResult(string title, int statusCode, ResultTypes resultType, IEnumerable<Notification> notifications)
        : this(title, statusCode, resultType, default, notifications)
    { }

    public ApiResult(string title, int statusCode, OperationResult<T> operationResult)
        : base(title, statusCode, operationResult)
    {
        Data = operationResult.Data;
    }

    #endregion CTOR
}

public class ApiMultipleResults<T> : ApiResult<IEnumerable<ResultItem<T?>>>
{
    #region CTOR

    /// <summary>
    /// CTOR vazio necessário para conversão de Json
    /// </summary>
    public ApiMultipleResults()
    { }

    public ApiMultipleResults(string title, int statusCode, OperationResultCollection<T> operationResultCollection)
        : base(title, statusCode, operationResultCollection.ResultType, operationResultCollection.Select(o => new ResultItem<T?>(o.Data, o.Notifications)).ToList())
    {
        if (Data?.Any() == true)
        {
            for (int i = 0; i < Data.Count(); i++)
            {
                // Se result.Notifications for vazio, então result.Notifications=null;
                var result = Data.ElementAt(i);
                if (!(result.Notifications?.Any() == true))
                {
                    result.Notifications = null;
                }
            }
        }
        else
        {
            Data = null;
        }
    }

    #endregion CTOR
}