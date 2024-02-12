namespace Maxsys.Core.Web.Exceptions;

/// <summary>
/// Representa um erro que ocorre quando o usuário não está logado.
/// </summary>
public class NotLoggedUserException : Exception
{
    private const string DEFAULT_MESSAGE = "User is not logged.";

    public NotLoggedUserException() : base(DEFAULT_MESSAGE)
    { }

    public NotLoggedUserException(string? message)
        : base(message ?? DEFAULT_MESSAGE)
    { }

    public NotLoggedUserException(string? message, Exception? innerException)
        : base(message ?? DEFAULT_MESSAGE, innerException)
    { }
}