using System.Linq.Expressions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Interfaces.Services;

public interface IService<TFilter> : IService
    where TFilter : IFilter
{
    #region GET

    /// <summary>NEW</summary>
    Task<TDestination?> GetAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion GET

    #region LIST

    /// <summary>NEW</summary>
    Task<ListDTO<TDestination>> GetListAsync<TDestination>(TFilter filters, ListCriteria criteria, Expression<Func<TDestination, dynamic>> keySelector, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>NEW</summary>
    Task<ListDTO<TDestination>> GetListAsync<TDestination>(TFilter filters, ListCriteria criteria, ISortColumnSelector<TDestination>? sortColumnSelector, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>NEW</summary>
    Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>NEW</summary>
    Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(TFilter filters, ListCriteria criteria, ISortColumnSelector<TDestination> sortColumnSelector, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>NEW</summary>
    Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(TFilter filters, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascendant, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion LIST

    #region QTY

    /// <summary>NEW</summary>
    Task<int> CountAsync(TFilter? filters, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    Task<bool> AnyAsync(TFilter? filters, CancellationToken cancellationToken = default);

    #endregion QTY
}