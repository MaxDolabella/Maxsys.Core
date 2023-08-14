namespace Maxsys.Core.Web;

public class DataWrapper<T>
{
    /// <summary>
    /// CTOR vazio necessário para conversão de Json
    /// </summary>
    public DataWrapper()
    { }

    public DataWrapper(T? data)
    {
        Data = data;
    }

    public T? Data { get; init; }
}

public class ApiResult
{
    public string Title { get; init; }
    public int Status { get; init; }
    public ResultTypes ResultType { get; init; }

    /// <summary>
    /// CTOR vazio necessário para conversão de Json
    /// </summary>
    public ApiResult()
    { }

    public ApiResult(string title, int status, ResultTypes resultType)
    {
        Title = title;
        ResultType = resultType;
        Status = status;
    }
}

public class ApiResult<T> : ApiResult
{
    public T? Result { get; init; }

    /// <summary>
    /// CTOR vazio necessário para conversão de Json
    /// </summary>
    public ApiResult()
    { }

    public ApiResult(string title, T? result, int status, ResultTypes resultType = ResultTypes.Success)
        : base(title, status, resultType)
    {
        Result = result;
    }
}

public class ApiDataResult<T> : ApiResult<DataWrapper<T>?>
{
    /// <summary>
    /// CTOR vazio necessário para conversão de Json
    /// </summary>
    public ApiDataResult()
    { }

    public ApiDataResult(string title, T? data, int status, ResultTypes resultType = ResultTypes.Success)
        : base(title, data is null ? null : new(data), status, resultType)
    { }
}

public class ApiListResult<T> : ApiResult<ListDTO<T>>
{
    /// <summary>
    /// CTOR vazio necessário para conversão de Json
    /// </summary>
    public ApiListResult()
    { }

    public ApiListResult(string title, ListDTO<T> data, int status, ResultTypes resultType = ResultTypes.Success)
        : base(title, data, status, resultType)
    { }
}