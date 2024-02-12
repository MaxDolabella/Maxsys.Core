using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;

namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Fornece uma interface básica para obtenção de dados.<br/>
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TListDTO"></typeparam>
/// <typeparam name="TFormDTO"></typeparam>
/// <typeparam name="TFilter"></typeparam>
public interface IService<TKey, TListDTO, TFormDTO, TFilter>
    : IService<TKey, TFilter>
    where TKey : notnull
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TFilter : IFilter, new()
{
#pragma warning disable CS1735 // XML comment has a typeparamref tag, but there is no type parameter by that name

    #region GET

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TFormDTO"/></remarks>
    Task<TFormDTO?> GetAsync(TKey id, CancellationToken cancellationToken = default);

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TFormDTO"/></remarks>
    Task<TFormDTO?> GetAsync(TFilter filters, CancellationToken cancellationToken = default);

    #endregion GET

    #region LIST

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TListDTO"/></remarks>
    Task<ListDTO<TListDTO>> GetListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default);

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TListDTO"/></remarks>
    Task<IReadOnlyList<TListDTO>> ToListAsync(TFilter filters, CancellationToken cancellationToken = default);

    #endregion LIST

#pragma warning restore CS1735 // XML comment has a typeparamref tag, but there is no type parameter by that name
}