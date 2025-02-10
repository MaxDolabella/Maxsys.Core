namespace Maxsys.Core.Exceptions;

/// <summary>
/// Representa um erro que ocorre quando o usuário não está logado.
/// </summary>
public class NotLoggedUserException : DomainException
{
    private const string DEFAULT_MESSAGE = "User is not logged.";

    public NotLoggedUserException() : base(DEFAULT_MESSAGE)
    { }

    public NotLoggedUserException(string message)
        : base(message)
    { }

    public NotLoggedUserException(string message, Exception innerException)
        : base(message, innerException)
    { }
}