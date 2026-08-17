using FluentValidation.Results;
using Maxsys.Core.Extensions;

namespace Maxsys.Core;

public partial class OperationResult
{
    [Obsolete("Utilize Result.Error(message)")]
    public OperationResult(string notificationMessage)
        : this(new Notification(notificationMessage))
    { }

    [Obsolete("Utilize Result.Error/Warning/Info(message, details)")]
    public OperationResult(string notificationMessage, string? notificationDetails, ResultTypes resultType = ResultTypes.Error)
        : this(new Notification(notificationMessage, notificationDetails, resultType))
    { }

    [Obsolete("Utilize Result.FromNotifications(validationResult.ConvertToNotifications())")]
    public OperationResult(ValidationResult validationResult)
    {
        Notifications = validationResult.ConvertToNotifications();
    }

    [Obsolete("Utilize o Result.FromException(Exception, ResultTypes).")]
    public OperationResult(Exception exception, ResultTypes resultType = ResultTypes.Error)
        : this(new Notification(exception, resultType))
    { }
}

public partial class OperationResult<T>
{
    [Obsolete("Utilize Result.Success(data)")]
    public OperationResult(T data)
        : this(data, default(List<Notification>?))
    { }

    [Obsolete("Utilize Result.Error/Warning/Info(message)")]
    public OperationResult(string notificationMessage, ResultTypes resultType = ResultTypes.Error)
       : this(default, notificationMessage, resultType)
    { }

    [Obsolete("Utilize Result.Error/Warning/Info(data, message)")]
    public OperationResult(T? data, string notificationMessage, ResultTypes resultType = ResultTypes.Error)
       : this(data, [new(notificationMessage, null, resultType)])
    { }

    [Obsolete("Utilize Result.FromNotifications<T>(validationResult.ConvertToNotifications())")]
    public OperationResult(ValidationResult validationResult)
        : this(default, validationResult.ConvertToNotifications())
    { }

    [Obsolete("Utilize Result.FromNotifications<T>(data, validationResult.ConvertToNotifications())")]
    public OperationResult(T? data, ValidationResult validationResult)
       : this(data, validationResult.ConvertToNotifications())
    { }

    [Obsolete("Utilize Result.FromException(exception, resultType)")]
    public OperationResult(Exception exception, ResultTypes resultType = ResultTypes.Error)
        : this(default, exception, resultType)
    { }

    [Obsolete("Utilize Result.FromException(exception, resultType).Cast<T>(data)")]
    public OperationResult(T? data, Exception exception, ResultTypes resultType = ResultTypes.Error)
        : base(exception, resultType)
    {
        Data = data;
    }
}
