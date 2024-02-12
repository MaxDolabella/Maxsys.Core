namespace Maxsys.Core.Web;

public abstract class ApiResultBase
{
    public string Title { get; set; } = string.Empty;
    public int StatusCode { get; set; } = 200;
    public ResultTypes ResultType { get; set; } = ResultTypes.Success;
    public string ResultTypeDescription => ResultType.ToString();

    public string? Tag { get; set; } = null;

    protected ApiResultBase(string title, int statusCode, ResultTypes resultType)
    {
        Title = title;
        ResultType = resultType;
        StatusCode = statusCode;

        Tag = null;
    }
}