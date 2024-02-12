using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;

namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Fornece uma interface básica para obtenção e alteração de dados.<br/>
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TReadDTO"></typeparam>
/// <typeparam name="TCreateDTO"></typeparam>
/// <typeparam name="TUpdateDTO"></typeparam>
/// <typeparam name="TFilter"></typeparam>
public interface IService<TKey, TReadDTO, TCreateDTO, TUpdateDTO, TFilter>
    : IService<TKey, TReadDTO, TReadDTO, TCreateDTO, TUpdateDTO, TFilter>
    where TKey : notnull
    where TReadDTO : class, IDTO
    where TCreateDTO : class, IDTO
    where TUpdateDTO : class, IDTO, IKey<TKey>
    where TFilter : IFilter, new()
{ }