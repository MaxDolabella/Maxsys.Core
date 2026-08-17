using System.Diagnostics.CodeAnalysis;
using Chinook.Api.Model.DTOs;
using Chinook.Api.Model.Services;
using Maxsys.Core;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.Api.Controllers;

[ApiController]
[Route("api/artists")]
public sealed class ArtistController(IArtistService service) : AppControllerBase
{
    private readonly IArtistService _service = service;

    [HttpGet]
    public async Task<IActionResult> List([FromJson, DisallowNull] ListCriteria? criteria, CancellationToken ct = default)
    {
        var result = await _service.GetListAsync<ArtistListDto>(criteria, ct);

        return ApiListResult(result);
    }
}
