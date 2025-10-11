using System.Text.Json.Serialization;

namespace Maxsys.Core.Web;

public abstract class ApiResultBase
{
    public const string MAXSYS_API = nameof(MaxsysAPI);

    [JsonPropertyOrder(-2147483648)]
    public string MaxsysAPI { get; } = MAXSYS_API;

    [JsonPropertyOrder(-2147483647)]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyOrder(-2147483646)]
    public int StatusCode { get; set; } = 200;

    [JsonPropertyOrder(-2147483645)]
    public ResultTypes ResultType { get; set; } = ResultTypes.Success;

    [JsonPropertyOrder(2147483647)]
    public string? Tag { get; set; } = null;

    protected ApiResultBase(string title, int statusCode, ResultTypes resultType)
    {
        Title = title;
        ResultType = resultType;
        StatusCode = statusCode;

        Tag = null;
    }
}