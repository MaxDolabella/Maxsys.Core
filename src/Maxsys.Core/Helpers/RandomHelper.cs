namespace Maxsys.Core.Helpers;

/// <summary>
/// Classe auxiliar para geração de valores aleatórios usando Random.Shared.
/// </summary>
public static class RandomHelper
{
    /// <summary>
    /// Gera um número inteiro aleatório dentro do intervalo especificado.
    /// </summary>
    /// <param name="min">Valor mínimo (inclusivo).</param>
    /// <param name="max">Valor máximo (exclusivo).</param>
    /// <returns>Um número inteiro aleatório maior ou igual a <paramref name="min"/> e menor que <paramref name="max"/>.</returns>
    /// <example>
    /// <code>
    /// int valor = RandomHelper.GetInt(1, 10); // Retorna um valor entre 1 e 9
    /// </code>
    /// </example>
    public static int GetInt(int min, int max)
        => Random.Shared.Next(min, max);

    /// <summary>
    /// Gera um número double aleatório dentro do intervalo especificado.
    /// </summary>
    /// <param name="min">Valor mínimo (inclusivo).</param>
    /// <param name="max">Valor máximo (inclusivo).</param>
    /// <returns>Um número double aleatório entre <paramref name="min"/> e <paramref name="max"/>.</returns>
    /// <example>
    /// <code>
    /// double valor = RandomHelper.GetDouble(5.0, 10.0); // Retorna um valor entre 5.0 e 10.0
    /// </code>
    /// </example>
    public static double GetDouble(double min, double max)
        => Random.Shared.NextDouble() * (max - min) + min;

    /// <summary>
    /// Gera um número double aleatório entre 0.0 e 1.0.
    /// </summary>
    /// <returns>Um número double aleatório maior ou igual a 0.0 e menor que 1.0.</returns>
    /// <example>
    /// <code>
    /// double valor = RandomHelper.GetDouble(); // Retorna um valor entre 0.0 e 1.0
    /// </code>
    /// </example>
    public static double GetDouble()
        => Random.Shared.NextDouble();

    /// <summary>
    /// Gera um valor aleatório dentro de uma margem simétrica em torno de zero.
    /// </summary>
    /// <param name="margin">A margem máxima (positiva ou negativa).</param>
    /// <returns>Um valor double aleatório entre -<paramref name="margin"/> e +<paramref name="margin"/>.</returns>
    /// <example>
    /// <code>
    /// double variacao = RandomHelper.GetMargin(5.0); // Retorna um valor entre -5.0 e +5.0
    /// </code>
    /// </example>
    public static double GetMargin(double margin)
        => Random.Shared.NextDouble() * (margin + margin) - margin;

    /// <summary>
    /// Adiciona uma variação aleatória a um número dentro de uma margem especificada.
    /// </summary>
    /// <param name="number">O número base.</param>
    /// <param name="margin">A margem de variação (positiva ou negativa).</param>
    /// <returns>O <paramref name="number"/> com uma variação aleatória entre -<paramref name="margin"/> e +<paramref name="margin"/>.</returns>
    /// <example>
    /// <code>
    /// double resultado = RandomHelper.GetMarginFromNumber(100.0, 10.0); 
    /// // Retorna um valor entre 90.0 e 110.0
    /// </code>
    /// </example>
    public static double GetMarginFromNumber(double number, double margin)
        => number + GetMargin(margin);

    /// <summary>
    /// Seleciona um item aleatório de uma lista.
    /// </summary>
    /// <typeparam name="T">O tipo dos elementos na lista.</typeparam>
    /// <param name="list">A lista da qual selecionar um item.</param>
    /// <returns>Um item aleatório da <paramref name="list"/>.</returns>
    /// <exception cref="ArgumentNullException">Lançada quando <paramref name="list"/> é null.</exception>
    /// <exception cref="ArgumentException">Lançada quando <paramref name="list"/> está vazia.</exception>
    /// <example>
    /// <code>
    /// var nomes = new List&lt;string&gt; { "Ana", "João", "Maria" };
    /// string nomeAleatorio = nomes.GetRandomItem(); // Retorna "Ana", "João" ou "Maria"
    /// </code>
    /// </example>
    public static T GetRandomItem<T>(this IList<T> list)
        => list[Random.Shared.Next(0, list.Count)];
}