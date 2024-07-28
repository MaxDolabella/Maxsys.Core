using System.Linq.Expressions;
using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Fornece uma interface básica para obtenção de dados.<br/>
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TListDTO"></typeparam>
/// <typeparam name="TFormDTO"></typeparam>
/// <typeparam name="TFilter"></typeparam>
public interface IService<TEntity, TKey, TListDTO, TFormDTO, TFilter>
    : IService<TEntity, TKey, TFilter>
    where TEntity : class
    where TKey : notnull
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TFilter : IFilter<TEntity>, new()
{
    #region GET

    Task<TFormDTO?> GetAsync(TKey id, CancellationToken cancellationToken = default);

    Task<TFormDTO?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<TFormDTO?> GetAsync(TFilter filters, CancellationToken cancellationToken = default);

    #endregion GET

    #region LIST

    Task<List<TListDTO>> ToListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<List<TListDTO>> ToListAsync(Expression<Func<TEntity, bool>> predicate, ListCriteria criteria, CancellationToken cancellationToken = default);

    Task<List<TListDTO>> ToListAsync(Expression<Func<TEntity, bool>> predicate, Pagination? pagination, Expression<Func<TListDTO, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    Task<List<TListDTO>> ToListAsync(TFilter filters, CancellationToken cancellationToken = default);

    Task<List<TListDTO>> ToListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default);

    Task<List<TListDTO>> ToListAsync(TFilter filters, Pagination? pagination, Expression<Func<TListDTO, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TListDTO"/></remarks>
    Task<ListDTO<TListDTO>> GetListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default);

    #endregion LIST
}