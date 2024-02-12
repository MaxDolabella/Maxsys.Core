using System.Linq.Expressions;

namespace Maxsys.Core.Extensions;

/// <summary>
/// Extensões para consultas LINQ que implementam um Left Outer Join.
/// </summary>
public static partial class QueryableExtensions
{
    /// <summary>
    /// Obrigado chat GPT.
    /// <para/>
    /// Executa um Left Outer Join entre duas sequências com base em chaves correspondentes
    /// e projeta o resultado usando um seletor de resultados.
    /// </summary>
    /// <typeparam name="TOuter">O tipo dos elementos na sequência externa.</typeparam>
    /// <typeparam name="TInner">O tipo dos elementos na sequência interna.</typeparam>
    /// <typeparam name="TKey">O tipo das chaves utilizadas para realizar o left outer join.</typeparam>
    /// <typeparam name="TResult">O tipo dos elementos no resultado da junção.</typeparam>
    /// <param name="outer">A sequência externa a ser unida.</param>
    /// <param name="inner">A sequência interna a ser unida.</param>
    /// <param name="outerKeySelector">Uma função para extrair o Left Outer Join de cada elemento da primeira sequência.</param>
    /// <param name="innerKeySelector">Uma função para extrair o Left Outer Join de cada elemento da segunda sequência.</param>
    /// <param name="resultSelector">O seletor de resultados que projeta os elementos da junção.</param>
    /// <remarks>
    /// Atalho para query.GroupJoin(...).SelectMany(...).Select(...)
    /// <example>
    /// <code>
    /// locations.LeftOuterJoin(countries,
    /// location => location.CountryId,
    /// country => country.Id,
    /// (outer, inner) => new { Location = outer, Country = inner }) //Country:null
    /// </code>
    /// </example>
    /// </remarks>
    /// <returns>Uma sequência de resultados do Left Outer Join .</returns>
    public static IQueryable<TResult> LeftOuterJoin<TOuter, TInner, TKey, TResult>(
        this IQueryable<TOuter> outer,
        IQueryable<TInner> inner,
        Expression<Func<TOuter, TKey>> outerKeySelector,
        Expression<Func<TInner, TKey>> innerKeySelector,
        Expression<Func<TOuter, TInner?, TResult>> resultSelector)
    {
        var convertedResultSelector = ConvertResultSelector<TOuter, TInner, TResult>(resultSelector);

        return outer.GroupJoin(inner,
           outerKeySelector,
           innerKeySelector,
           (outer, innerList) => new
           {
               Outer = outer,
               InnerList = innerList
           })
             .SelectMany(a => a.InnerList.DefaultIfEmpty(),
                 (a, innerItem) => new LeftOuterJoinResult<TOuter, TInner?> { Outer = a.Outer, Inner = innerItem })
             .Select(ConvertResultSelector(resultSelector));
    }

    /// <summary>
    /// Obrigado chat GPT.
    /// <para/>
    /// Executa um Left Outer Join entre duas sequências com base em chaves correspondentes
    /// e projeta o resultado usando um seletor de resultados.
    /// </summary>
    /// <typeparam name="TOuter">O tipo dos elementos na sequência externa.</typeparam>
    /// <typeparam name="TInner">O tipo dos elementos na sequência interna.</typeparam>
    /// <typeparam name="TKey">O tipo das chaves utilizadas para realizar o left outer join.</typeparam>
    /// <typeparam name="TResult">O tipo dos elementos no resultado da junção.</typeparam>
    /// <param name="outer">A sequência externa a ser unida.</param>
    /// <param name="inner">A sequência interna a ser unida.</param>
    /// <param name="outerKeySelector">Uma função para extrair o Left Outer Join de cada elemento da primeira sequência.</param>
    /// <param name="innerKeySelector">Uma função para extrair o Left Outer Join de cada elemento da segunda sequência.</param>
    /// <param name="resultSelector">O seletor de resultados que projeta os elementos da junção.</param>
    /// <remarks>
    /// Atalho para query.GroupJoin(...).Select(...)
    /// <example>
    /// <code>
    /// countries.LeftOuterJoin(location,
    /// country => country.Id,
    /// location => location.CountryId,
    /// (outer, innerList) => new { Country = outer, Locations = innerList }) //IEnumerable&lt;Location&gt;
    /// </code>
    /// </example>
    /// </remarks>
    /// <returns>Uma sequência de resultados do Left Outer Join .</returns>
    public static IQueryable<TResult> LeftOuterJoinList<TOuter, TInner, TKey, TResult>(
        this IQueryable<TOuter> outer,
        IQueryable<TInner> inner,
        Expression<Func<TOuter, TKey>> outerKeySelector,
        Expression<Func<TInner, TKey>> innerKeySelector,
        Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector)
    {
        var convertedResultSelector = ConvertResultSelector<TOuter, TInner, TResult>(resultSelector);

        return outer.GroupJoin(inner,
            outerKeySelector,
            innerKeySelector,
            (outer, innerList) => new LeftOuterJoinListResult<TOuter, TInner>
            {
                Outer = outer,
                InnerList = innerList
            })
            .Select(ConvertResultSelector(resultSelector));
    }

    /// <summary>
    /// Converte o seletor de resultados original para o tipo esperado na junção externa à esquerda.
    /// </summary>
    /// <typeparam name="TOuter">O tipo dos elementos na sequência externa.</typeparam>
    /// <typeparam name="TInner">O tipo dos elementos na sequência interna.</typeparam>
    /// <typeparam name="TResult">O tipo dos elementos no resultado da junção.</typeparam>
    /// <param name="resultSelector">O seletor de resultados original.</param>
    /// <returns>O seletor de resultados convertido para o tipo de junção externa à esquerda.</returns>
    private static Expression<Func<LeftOuterJoinResult<TOuter, TInner?>, TResult>> ConvertResultSelector<TOuter, TInner, TResult>(
        Expression<Func<TOuter, TInner?, TResult>> resultSelector)
    {
        var leftOuterJoinResultParam = Expression.Parameter(typeof(LeftOuterJoinResult<TOuter, TInner?>), "result");
        var outerParam = Expression.Property(leftOuterJoinResultParam, nameof(LeftOuterJoinResult<TOuter, TInner?>.Outer));
        var innerParam = Expression.Property(leftOuterJoinResultParam, nameof(LeftOuterJoinResult<TOuter, TInner?>.Inner));

        var body = RebindParameters(resultSelector.Body, outerParam, innerParam);

        return Expression.Lambda<Func<LeftOuterJoinResult<TOuter, TInner?>, TResult>>(body, leftOuterJoinResultParam);
    }

    /// <summary>
    /// Converte o seletor de resultados original para o tipo esperado na junção externa à esquerda.
    /// </summary>
    /// <typeparam name="TOuter">O tipo dos elementos na sequência externa.</typeparam>
    /// <typeparam name="TInner">O tipo dos elementos na sequência interna.</typeparam>
    /// <typeparam name="TResult">O tipo dos elementos no resultado da junção.</typeparam>
    /// <param name="resultSelector">O seletor de resultados original.</param>
    /// <returns>O seletor de resultados convertido para o tipo de junção externa à esquerda.</returns>
    private static Expression<Func<LeftOuterJoinListResult<TOuter, TInner>, TResult>> ConvertResultSelector<TOuter, TInner, TResult>(
        Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector)
    {
        var leftOuterJoinResultParam = Expression.Parameter(typeof(LeftOuterJoinListResult<TOuter, TInner>), "result");
        var outerParam = Expression.Property(leftOuterJoinResultParam, nameof(LeftOuterJoinListResult<TOuter, TInner>.Outer));
        var innerParam = Expression.Property(leftOuterJoinResultParam, nameof(LeftOuterJoinListResult<TOuter, TInner>.InnerList));

        var body = RebindParameters(resultSelector.Body, outerParam, innerParam);

        return Expression.Lambda<Func<LeftOuterJoinListResult<TOuter, TInner>, TResult>>(body, leftOuterJoinResultParam);
    }

    /// <summary>
    /// Substitui os parâmetros em uma expressão com novos parâmetros fornecidos.
    /// </summary>
    /// <param name="expression">A expressão original.</param>
    /// <param name="newParameters">Os novos parâmetros a serem utilizados na substituição.</param>
    /// <returns>A expressão resultante após a substituição dos parâmetros.</returns>
    private static Expression RebindParameters(Expression expression, params Expression[] newParameters)
    {
        return new ParameterRebinder(newParameters).Visit(expression);
    }

    /// <summary>
    /// Classe interna para substituição de parâmetros em uma expressão.
    /// </summary>
    private class ParameterRebinder : ExpressionVisitor
    {
        private readonly Expression[] _newParameters;

        /// <summary>
        /// Inicializa uma nova instância da classe ParameterRebinder.
        /// </summary>
        /// <param name="newParameters">Os novos parâmetros a serem utilizados na substituição.</param>
        public ParameterRebinder(Expression[] newParameters)
        {
            _newParameters = newParameters;
        }

        /// <summary>
        /// Substitui um parâmetro na expressão, se necessário.
        /// </summary>
        /// <param name="node">O nó de parâmetro da expressão.</param>
        /// <returns>A expressão resultante após a substituição do parâmetro.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            // Substitui os parâmetros pelos novos, se houver correspondência de tipo
            return _newParameters.FirstOrDefault(p => p.Type == node.Type) ?? node;
        }
    }

    /// <summary>
    /// Classe auxiliar para realização de Left Outer Join
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TInner"></typeparam>
    internal class LeftOuterJoinResult<TSource, TInner>
    {
        public required TSource Outer { get; set; }
        public required TInner? Inner { get; set; }
    }

    /// <summary>
    /// Classe auxiliar para realização de Left Outer Join
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TInner"></typeparam>
    internal class LeftOuterJoinListResult<TSource, TInner>
    {
        public required TSource Outer { get; set; }
        public required IEnumerable<TInner> InnerList { get; set; }
    }
}