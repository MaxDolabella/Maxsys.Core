using Maxsys.Core.DTO;

namespace Maxsys.Core;

public class InfoDTO<T> : IDTO, IKey<T> where T : notnull
{
    public InfoDTO()
    { }

    public InfoDTO(T id, string description, string? abbreviation = null)
    { 
        Id = id;
        Description = description;
        Abbreviation = abbreviation;
    }

    public T Id { get; set; }
    public string Description { get; set; }
    public string? Abbreviation { get; set; }
}