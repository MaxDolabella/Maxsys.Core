using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;

namespace Maxsys.Core.Interfaces.Services;

public interface IService<TKey, TReadDTO, TCreateDTO, TUpdateDTO, TFilter>
    : IService<TKey, TReadDTO, TReadDTO, TCreateDTO, TUpdateDTO, TFilter>
    where TKey : notnull
    where TReadDTO : class, IDTO
    where TCreateDTO : class, IDTO
    where TUpdateDTO : class, IDTO, IKey<TKey>
    where TFilter : IFilter, new()
{ }