using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;

namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Uma vez que a implementação dessa interface
/// utiliza-se de ProjectTo (automapper) na chamada do repositório,
/// é necessário garantir que os seguintes mapeamentos estão definidos:
/// <list type="bullet">
/// <item><description>Entidade -> <typeparamref name="TFormDTO"/></description></item>
/// <item><description>Entidade -> <typeparamref name="TListDTO"/></description></item>
/// </list>
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TListDTO"></typeparam>
/// <typeparam name="TFormDTO"></typeparam>
/// <typeparam name="TFilter"></typeparam>
public interface IEntityReadService<TKey, TListDTO, TFormDTO, TFilter> : IReadService<TFilter>
    where TKey : notnull
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TFilter : IFilter, new()
{
    /// <summary>
    /// Mapeamento necessário: Entidade -> <typeparamref name="TFormDTO"/>
    /// </summary>
    Task<TFormDTO?> GetAsync(object id, CancellationToken cancellation = default);

    /// <summary>
    /// Mapeamento necessário: Entidade -> <typeparamref name="TListDTO"/>
    /// </summary>
    Task<ListDTO<TListDTO>> GetListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellation = default);

    /// <summary>
    /// Mapeamento necessário: Entidade -> <see cref="InfoDTO{TKey}"/>
    /// </summary>
    Task<ListDTO<InfoDTO<TKey>>> GetInfoAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellation = default);
}