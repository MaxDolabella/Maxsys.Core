using AutoMapper;
using AutoMapper.QueryableExtensions;
using Maxsys.Core.Interfaces.Mapping;

namespace Maxsys.Mapping;

/// <summary>
/// Adaptador AutoMapper para as abstrações de mapeamento Maxsys:
/// <see cref="IObjectMapper"/> (instâncias) e <see cref="IQueryProjector"/> (projeção de queryable via <c>ProjectTo</c>).
/// </summary>
internal sealed class AutoMapperAdapter(IMapper mapper) : IObjectMapper, IQueryProjector
{
    public TDestination Map<TDestination>(object source)
        => mapper.Map<TDestination>(source);

    public TDestination Map<TDestination>(object source, Action<TDestination> afterMap)
    {
        var destination = mapper.Map<TDestination>(source);

        afterMap(destination);

        return destination;
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        => mapper.Map(source, destination);

    public IQueryable<TDestination> Project<TDestination>(IQueryable source)
        => source.ProjectTo<TDestination>(mapper.ConfigurationProvider);
}
