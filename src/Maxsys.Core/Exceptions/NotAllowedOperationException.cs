namespace Maxsys.Core.Exceptions;

/// <summary>
/// Representa um erro que ocorre ao se tentar realizar uma operação não permitida.
/// </summary>
public sealed class NotAllowedOperationException : Exception
{
    public NotAllowedOperationException(string message)
      : base($"Not Allowed Operation. {message}")
    {
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));
    }

    public NotAllowedOperationException(string message, Exception innerException)
      : base($"Not Allowed Operation. {message}", innerException)
    {
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));
        ArgumentNullException.ThrowIfNull(innerException, nameof(innerException));
    }
}