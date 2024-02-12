using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;

namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Fornece uma interface básica para obtenção de dados.<br/>
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDTO"></typeparam>
/// <typeparam name="TFilter"></typeparam>
public interface IService<TKey, TDTO, TFilter> : IService<TKey, TDTO, TDTO, TFilter>
    where TKey : notnull
    where TDTO : class, IDTO
    where TFilter : IFilter, new()
{ }
