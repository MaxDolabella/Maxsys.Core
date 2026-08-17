using AutoMapper;
using Chinook.Api.Model.DTOs;
using Chinook.Api.Model.Entities;

namespace Chinook.Api.Model.Mappers;

public sealed class Mappers : Profile
{
    public Mappers()
    {
        CreateMap<Artist, ArtistListDto>().ForMember(dst => dst.Id, cfg => cfg.MapFrom(src => src.ArtistId));
    }
}
