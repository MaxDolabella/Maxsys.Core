using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;

namespace Maxsys.Core.Interfaces.Services;

// remover em v18
[Obsolete("Use IService<TEntity, TKey, TFilter> instead.", true)]
public interface IService<TEntity, TKey, TListDTO, TFormDTO, TCreateDTO, TUpdateDTO, TFilter>
    : IService<TEntity, TKey, TListDTO, TFormDTO, TFilter>
    where TEntity : class
    where TKey : notnull
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TCreateDTO : class, IDTO
    where TUpdateDTO : class, IDTO, IKey<TKey>
    where TFilter : IFilter<TEntity>, new()
{
}