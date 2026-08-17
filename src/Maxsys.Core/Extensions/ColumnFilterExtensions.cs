using System.Linq.Expressions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Helpers;

namespace Maxsys.Core.Extensions;

/// <summary>
/// Métodos de extensão para facilitar a composição de listas de <see cref="ColumnFilter"/>
/// usando expressions fortemente tipadas em vez de strings literais.
/// </summary>
public static class ColumnFilterExtensions
{
    extension(List<ColumnFilter> filters)
    {
        /// <summary>
        /// Adiciona um <see cref="ColumnFilter"/> à lista usando uma expression para identificar
        /// o campo, evitando strings literais e garantindo segurança em tempo de compilação.
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo ao qual o filtro se aplica.</typeparam>
        /// <param name="property">
        /// Expression que identifica a propriedade a ser filtrada.
        /// Suporta dot notation para propriedades aninhadas: <c>x => x.Address.State.Name</c>.
        /// </param>
        /// <param name="value">
        /// Valor a ser comparado. Pode ser <see langword="null"/> quando o valor será definido posteriormente.
        /// Para <see cref="FilterMatchModes.Between"/>, espera-se um array <c>[min, max]</c>.
        /// Para <see cref="FilterMatchModes.In"/> e <see cref="FilterMatchModes.NotIn"/>, espera-se um array de valores.
        /// </param>
        /// <param name="matchMode">Modo de comparação a ser aplicado pelo filtro. Padrão: <see cref="FilterMatchModes.Equals"/>.</param>
        /// <example>
        /// <code>
        /// var filters = new List&lt;ColumnFilter&gt;();
        ///
        /// filters.AddFilter&lt;Order&gt;(x => x.Status,                FilterMatchModes.Equals,     OrderStatus.Active);
        /// filters.AddFilter&lt;Order&gt;(x => x.Customer.Name,         FilterMatchModes.Contains,   "Silva");
        /// filters.AddFilter&lt;Order&gt;(x => x.Customer.Address.City, FilterMatchModes.StartsWith, "São");
        /// filters.AddFilter&lt;Order&gt;(x => x.Total,                 FilterMatchModes.Gte,        1000);
        /// </code>
        /// </example>
        public void AddFilter<TModel>(Expression<Func<TModel, dynamic>> property, object? value, FilterMatchModes matchMode = FilterMatchModes.Equals)
        {
            filters.Add(new ColumnFilter
            {
                Field = ExpressionHelper.GetMemberPath(property),
                Value = value,
                MatchMode = matchMode
            });
        }
    }
}