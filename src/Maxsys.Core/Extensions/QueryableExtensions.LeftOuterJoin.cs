using System.Linq.Expressions;

namespace Maxsys.Core.Extensions;

/// <summary>
/// Extensões para consultas LINQ que implementam um Left Outer Join.
/// </summary>
public static partial class QueryableExtensions
{
    extension<TOuter>(IQueryable<TOuter> outer)
    {
        /// <summary>
        /// Executa um Left Outer Join entre duas sequências com base em chaves correspondentes
        /// e projeta o resultado usando um seletor de resultados.
        /// </summary>
        /// <typeparam name="TInner">O tipo dos elementos na sequência interna.</typeparam>
        /// <typeparam name="TKey">O tipo das chaves utilizadas para realizar o left outer join.</typeparam>
        /// <typeparam name="TResult">O tipo dos elementos no resultado da junção.</typeparam>
        /// <param name="inner">A sequência interna a ser unida.</param>
        /// <param name="outerKeySelector">Uma função para extrair a chave de cada elemento da primeira sequência.</param>
        /// <param name="innerKeySelector">Uma função para extrair a chave de cada elemento da segunda sequência.</param>
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
        /// <returns>Uma sequência de resultados do Left Outer Join.</returns>
        public IQueryable<TResult> LeftOuterJoin<TInner, TKey, TResult>(
            IQueryable<TInner> inner,
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<TOuter, TInner?, TResult>> resultSelector)
        {
            return outer.GroupJoin(inner,
               outerKeySelector,
               innerKeySelector,
               (o, innerList) => new
               {
                   Outer = o,
                   InnerList = innerList
               })
                 .SelectMany(a => a.InnerList.DefaultIfEmpty(),
                     (a, innerItem) => new LeftOuterJoinResult<TOuter, TInner?> { Outer = a.Outer, Inner = innerItem })
                 .Select(ConvertResultSelector(resultSelector));
        }

        /// <summary>
        /// Executa um Left Outer Join entre duas sequências com base em chaves correspondentes
        /// e projeta o resultado usando um seletor de resultados com lista.
        /// </summary>
        /// <typeparam name="TInner">O tipo dos elementos na sequência interna.</typeparam>
        /// <typeparam name="TKey">O tipo das chaves utilizadas para realizar o left outer join.</typeparam>
        /// <typeparam name="TResult">O tipo dos elementos no resultado da junção.</typeparam>
        /// <param name="inner">A sequência interna a ser unida.</param>
        /// <param name="outerKeySelector">Uma função para extrair a chave de cada elemento da primeira sequência.</param>
        /// <param name="innerKeySelector">Uma função para extrair a chave de cada elemento da segunda sequência.</param>
        /// <param name="resultSelector">O seletor de resultados que projeta os elementos da junção.</param>
        /// <remarks>
        /// Atalho para query.GroupJoin(...).Select(...)
        /// <example>
        /// <code>
        /// countries.LeftOuterJoinList(location,
        /// country => country.Id,
        /// location => location.CountryId,
        /// (outer, innerList) => new { Country = outer, Locations = innerList }) //IEnumerable&lt;Location&gt;
        /// </code>
        /// </example>
        /// </remarks>
        /// <returns>Uma sequência de resultados do Left Outer Join.</returns>
        public IQueryable<TResult> LeftOuterJoinList<TInner, TKey, TResult>(
            IQueryable<TInner> inner,
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector)
        {
            return outer.GroupJoin(inner,
                outerKeySelector,
                innerKeySelector,
                (o, innerList) => new LeftOuterJoinListResult<TOuter, TInner>
                {
                    Outer = o,
                    InnerList = innerList
                })
                .Select(ConvertResultSelector(resultSelector));
        }
    }

    #region LeftOuterJoin (private)

    private static Expression<Func<LeftOuterJoinResult<TOuter, TInner?>, TResult>> ConvertResultSelector<TOuter, TInner, TResult>(
        Expression<Func<TOuter, TInner?, TResult>> resultSelector)
    {
        var leftOuterJoinResultParam = Expression.Parameter(typeof(LeftOuterJoinResult<TOuter, TInner?>), "result");
        var outerParam = Expression.Property(leftOuterJoinResultParam, nameof(LeftOuterJoinResult<TOuter, TInner?>.Outer));
        var innerParam = Expression.Property(leftOuterJoinResultParam, nameof(LeftOuterJoinResult<TOuter, TInner?>.Inner));

        var body = RebindParameters(resultSelector.Body, outerParam, innerParam);

        return Expression.Lambda<Func<LeftOuterJoinResult<TOuter, TInner?>, TResult>>(body, leftOuterJoinResultParam);
    }

    private static Expression<Func<LeftOuterJoinListResult<TOuter, TInner>, TResult>> ConvertResultSelector<TOuter, TInner, TResult>(
        Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector)
    {
        var leftOuterJoinResultParam = Expression.Parameter(typeof(LeftOuterJoinListResult<TOuter, TInner>), "result");
        var outerParam = Expression.Property(leftOuterJoinResultParam, nameof(LeftOuterJoinListResult<TOuter, TInner>.Outer));
        var innerParam = Expression.Property(leftOuterJoinResultParam, nameof(LeftOuterJoinListResult<TOuter, TInner>.InnerList));

        var body = RebindParameters(resultSelector.Body, outerParam, innerParam);

        return Expression.Lambda<Func<LeftOuterJoinListResult<TOuter, TInner>, TResult>>(body, leftOuterJoinResultParam);
    }

    private static Expression RebindParameters(Expression expression, params Expression[] newParameters)
    {
        return new ParameterRebinder(newParameters).Visit(expression);
    }

    private class ParameterRebinder : ExpressionVisitor
    {
        private readonly Expression[] _newParameters;

        public ParameterRebinder(Expression[] newParameters)
        {
            _newParameters = newParameters;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _newParameters.FirstOrDefault(p => p.Type == node.Type) ?? node;
        }
    }

    #endregion LeftOuterJoin (private)

    /// <summary>
    /// Classe auxiliar para realização de Left Outer Join
    /// </summary>
    public class LeftOuterJoinResult<TSource, TInner>
    {
        public required TSource Outer { get; set; }
        public required TInner? Inner { get; set; }
    }

    /// <summary>
    /// Classe auxiliar para realização de Left Outer Join
    /// </summary>
    public class LeftOuterJoinListResult<TSource, TInner>
    {
        public required TSource Outer { get; set; }
        public required IEnumerable<TInner> InnerList { get; set; }
    }
}