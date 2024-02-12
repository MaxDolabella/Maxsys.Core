using System.Linq.Expressions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Fornece uma interface básica para obtenção de dados.<br/>
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TFilter"></typeparam>
public interface IService<TKey, TFilter> : IService<TFilter>
    where TKey : notnull
    where TFilter : IFilter
{
#pragma warning disable CS1735 // XML comment has a typeparamref tag, but there is no type parameter by that name

    #region GET

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/></remarks>
    Task<TDestination?> GetAsync<TDestination>(TKey id, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion GET

    #region LIST

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <see cref="InfoDTO{TKey}"/> </remarks>
    Task<ListDTO<InfoDTO<TKey>>> GetInfoListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default);

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <see cref="InfoDTO{TKey}"/> </remarks>
    Task<IReadOnlyList<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, CancellationToken cancellationToken = default);

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <see cref="InfoDTO{TKey}"/> </remarks>
    Task<IReadOnlyList<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default);

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <see cref="InfoDTO{TKey}"/> </remarks>
    Task<IReadOnlyList<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, Pagination? pagination, Expression<Func<InfoDTO<TKey>, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascendant, CancellationToken cancellationToken = default);

    #endregion LIST

#pragma warning restore CS1735 // XML comment has a typeparamref tag, but there is no type parameter by that name
}