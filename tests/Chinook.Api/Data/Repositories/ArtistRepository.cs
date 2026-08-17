using Chinook.Api.Data.Context;
using Chinook.Api.Model.Entities;
using Chinook.Api.Model.Repositories;
using Maxsys.Core.Interfaces.Mapping;
using Maxsys.Data;

namespace Chinook.Api.Data.Repositories;

public class ArtistRepository : RepositoryBase<Artist>, IArtistRepository
{
    public ArtistRepository(ChinookDbContext context, IQueryProjector mapper) : base(context, mapper)
    { }
}
