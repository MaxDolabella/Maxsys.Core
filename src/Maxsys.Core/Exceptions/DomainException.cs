namespace Maxsys.Core.Exceptions;

/// <summary>
/// Representa um erro de domínio.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message)
        : base(message) 
    {
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));
    }

    public DomainException(string message, Exception innerException)
      : base(message, innerException)
    {
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));
        ArgumentNullException.ThrowIfNull(innerException, nameof(innerException));
    }
}