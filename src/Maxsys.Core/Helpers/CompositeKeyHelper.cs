namespace Maxsys.Core.Helpers;

/// <summary>
/// Utilitário para construção de arrays de chaves compostas compatíveis com
/// <see cref="Interfaces.Repositories.IRepository{TEntity}"/> (<c>GetByIdAsync</c>, <c>IdExistsAsync</c>, <c>DeleteAsync</c>).
/// </summary>
/// <example>
/// <code>
/// await repo.GetByIdAsync(CompositeKeyHelper.Of(orderId, lineId));
/// await repo.IdExistsAsync(CompositeKeyHelper.Of(orderId, lineId));
/// await repo.DeleteAsync(CompositeKeyHelper.Of(orderId, lineId));
/// </code>
/// </example>
public static class CompositeKeyHelper
{
    /// <summary>
    /// Cria um array de valores de chave para uso em operações de repositório com chave composta.
    /// </summary>
    /// <param name="keys">Os valores individuais de cada componente da chave primária, na mesma ordem configurada no EF Core.</param>
    public static object[] Of(params object[] keys) => keys;
}
