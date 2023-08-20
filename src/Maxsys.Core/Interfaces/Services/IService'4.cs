using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;

namespace Maxsys.Core.Interfaces.Services;

public interface IService<TKey, TListDTO, TFormDTO, TFilter>
    : IService<TFilter>
    where TKey : notnull
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TFilter : IFilter, new()
{
    #region GET

    /// <summary>NEW</summary>
    Task<TFormDTO?> GetAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>NEW</summary>
    Task<TFormDTO?> GetAsync(TFilter filters, CancellationToken cancellationToken = default);

    #endregion GET

    #region LIST

    /// <summary>NEW</summary>
    Task<ListDTO<TListDTO>> GetListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default);

    #endregion LIST
}