using System.Linq.Expressions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Fornece uma interface para obtenção de dados.<br/>
/// </summary>
public interface IService<TKey, TFilter> : IService<TFilter>
    where TKey : notnull
    where TFilter : IFilter
{
    #region GET

    /// <summary>NEW</summary>
    Task<TDestination?> GetAsync<TDestination>(TKey id, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion GET

    #region LIST

    /// <summary>NEW</summary>
    Task<ListDTO<InfoDTO<TKey>>> GetInfoListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    Task<IReadOnlyList<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    Task<IReadOnlyList<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    Task<IReadOnlyList<InfoDTO<TKey>>> ToInfoListAsync(TFilter filters, Pagination? pagination, Expression<Func<InfoDTO<TKey>, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascendant, CancellationToken cancellationToken = default);

    #endregion LIST
}