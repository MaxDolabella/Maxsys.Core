namespace Maxsys.Core.Exceptions;

/// <summary>
/// Representa um erro que ocorre ao tentar validar um ServiceProvider.
/// </summary>
public sealed class InvalidServiceProviderException : Exception
{
    private const string DEFAULT_MESSAGE = "One or more errors has occurred while validating ServiceProvider:";

    /// <param name="errors">Lista de erros ocorridos.</param>
    public InvalidServiceProviderException(List<string> errors)
        : base(GetMessage(errors))
    { }

    private static string GetMessage(List<string> errors)
    {
        return $"{DEFAULT_MESSAGE}\r\n{string.Join("\r\n", errors)}";
    }
}