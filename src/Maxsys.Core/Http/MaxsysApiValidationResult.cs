using System.Net;

namespace Maxsys.Core.Http;

public class MaxsysApiValidationResult
{
    #region PROPS

    public HttpStatusCode HttpStatusCode { get; init; }
    public string ErrorMessage { get; init; }
    public string Content { get; init; }
    public Exception? Exception { get; set; }

    public bool IsValid => !string.IsNullOrWhiteSpace(ErrorMessage);

    #endregion PROPS

    #region CTOR

    private MaxsysApiValidationResult(HttpStatusCode httpStatusCode, string errorMessage, string content)
    {
        HttpStatusCode = httpStatusCode;
        ErrorMessage = errorMessage;
        Content = content;
    }

    private MaxsysApiValidationResult() : this(default, default!, default!)
    { }

    #endregion CTOR

    #region FACTORY

    public static MaxsysApiValidationResult CreateInvalidResult(HttpStatusCode httpStatusCode, string errorMessage, string content = "", Exception? exception = null)
    {
        return new MaxsysApiValidationResult(httpStatusCode, errorMessage, content)
        {
            Exception = exception
        };
    }

    public static MaxsysApiValidationResult CreateValidResult()
    {
        return new();
    }

    #endregion FACTORY

    #region METHODS

    public OperationResult ToOperationResult()
    {
        if (IsValid)
        {
            return new();
        }

        var message = $"Invalid Maxsys API validation\nHttpStatusCode:<{(int)HttpStatusCode}>\nError Message:{ErrorMessage}\nContent:{Content}".Trim();
        Notification notification = Exception is not null
            ? new(Exception, message)
            : new(message);

        return new(notification);
    }


    public OperationResult<T> ToOperationResult<T>()
    {
        return ToOperationResult().Cast<T>();
    }

    #endregion METHODS
}