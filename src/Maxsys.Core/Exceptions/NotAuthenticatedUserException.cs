namespace Maxsys.Core.Exceptions;

/// <summary>
/// Representa um erro que ocorre quando o usuário não está autenticado.
/// </summary>
public class NotAuthenticatedUserException : DomainException
{
    private const string DEFAULT_MESSAGE = "User is not authenticated.";

    public NotAuthenticatedUserException() : base(DEFAULT_MESSAGE)
    { }

    public NotAuthenticatedUserException(string message)
        : base(message)
    { }

    public NotAuthenticatedUserException(string message, Exception innerException)
        : base(message, innerException)
    { }
}