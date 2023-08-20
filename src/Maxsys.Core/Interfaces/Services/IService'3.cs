using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;

namespace Maxsys.Core.Interfaces.Services;

public interface IService<TKey, TDTO, TFilter> : IService<TKey, TDTO, TDTO, TFilter>
    where TKey : notnull
    where TDTO : class, IDTO
    where TFilter : IFilter, new()
{ }