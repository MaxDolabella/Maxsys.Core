using System.Linq.Expressions;
using Chinook.Api.Model.Entities;
using Chinook.Api.Model.Repositories;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Interfaces.Mapping;
using Maxsys.Core.Services;

namespace Chinook.Api.Model.Services;

public sealed class ArtistService(
    IArtistRepository repository,
    IUnitOfWork uow,
    IObjectMapper mapper) : ModelServiceBase<Artist, IArtistRepository, int>(repository, uow, mapper), IArtistService
{
    protected override Expression<Func<Artist, bool>> IdSelector(int id) => x => x.ArtistId == id;
}
