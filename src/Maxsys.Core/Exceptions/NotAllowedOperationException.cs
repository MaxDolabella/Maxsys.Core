namespace Maxsys.Core.Exceptions;

/// <summary>
/// Representa um erro que ocorre ao se tentar realizar uma operação não permitida.
/// </summary>
public sealed class NotAllowedOperationException : DomainException
{
    private const string BASE_MESSAGE = "Not Allowed Operation.";
    public NotAllowedOperationException(string message)
      : base($"{BASE_MESSAGE} {message}")
    { }

    public NotAllowedOperationException(string message, Exception innerException)
      : base($"{BASE_MESSAGE} {message}", innerException)
    { }
}