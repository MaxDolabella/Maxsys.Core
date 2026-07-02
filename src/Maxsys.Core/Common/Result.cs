namespace Maxsys.Core;

/// <summary>
/// Factory estática para criação de <see cref="OperationResult"/> e <see cref="OperationResult{T}"/>.
/// Não-Success nunca carrega <c>Data</c>: a versão tipada sempre retorna com <c>Data = default</c>.
/// Para retornar dado junto com Warning/Info, use <see cref="Success{T}(T)"/> combinado com
/// <c>AddWarningNotification</c>/<c>AddNotification</c> existentes em <see cref="OperationResult"/>.
/// </summary>
public static class Result
{
    // ---------- Success ----------

    public static OperationResult Success()
        => new();

    public static OperationResult<T> Success<T>(T? data)
        => new(data, default(List<Notification>?));

    // ---------- Info ----------

    public static OperationResult Info(string message, string? details = null, object? tag = null)
        => new(Build(message, details, tag, ResultTypes.Info));

    public static OperationResult<T> Info<T>(string message, string? details = null, object? tag = null)
        => new(Build(message, details, tag, ResultTypes.Info));

    // ---------- Warning ----------

    public static OperationResult Warning(string message, string? details = null, object? tag = null)
        => new(Build(message, details, tag, ResultTypes.Warning));

    public static OperationResult<T> Warning<T>(string message, string? details = null, object? tag = null)
        => new(Build(message, details, tag, ResultTypes.Warning));

    // ---------- Error ----------

    public static OperationResult Error(string message, string? details = null, object? tag = null)
        => new(Build(message, details, tag, ResultTypes.Error));

    public static OperationResult<T> Error<T>(string message, string? details = null, object? tag = null)
        => new(Build(message, details, tag, ResultTypes.Error));

    // ---------- Exception ----------

    public static OperationResult FromException(Exception exception, ResultTypes resultType = ResultTypes.Error)
        => new(Build(exception, null, resultType));

    public static OperationResult<T> FromException<T>(Exception exception, ResultTypes resultType = ResultTypes.Error)
        => new(Build(exception, null, resultType));

    public static OperationResult FromException(Exception exception, string message, ResultTypes resultType = ResultTypes.Error)
        => new(Build(exception, message, resultType));

    public static OperationResult<T> FromException<T>(Exception exception, string message, ResultTypes resultType = ResultTypes.Error)
        => new(Build(exception, message, resultType));

    // ---------- FromNotifications ----------

    public static OperationResult FromNotifications(List<Notification> notifications)
        => new(notifications);

    public static OperationResult<T> FromNotifications<T>(List<Notification> notifications)
        => new(notifications);

    public static OperationResult<T> FromNotifications<T>(T? data, List<Notification> notifications)
        => new(data, notifications);

    // ---------- Build Notification ----------

    private static Notification Build(string message, string? details, object? tag, ResultTypes resultType)
        => new(message, details, resultType) { Tag = tag };

    private static Notification Build(Exception exception, string? message, ResultTypes resultType)
        => string.IsNullOrWhiteSpace(message) ? new(exception, resultType) : new(exception, message, resultType);
}