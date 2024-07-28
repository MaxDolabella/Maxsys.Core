namespace Maxsys.Core;

[Serializable]
public sealed class AddedEntityEventArgs<TEntity, TCreateDTO> : ModifiedEntityEventArgs<TEntity, TCreateDTO>
{
    public AddedEntityEventArgs(TEntity entity, TCreateDTO createDTO) : base(entity, createDTO)
    { }
}

[Serializable]
public sealed class UpdatedEntityEventArgs<TEntity, TUpdateDTO> : ModifiedEntityEventArgs<TEntity, TUpdateDTO>
{
    public UpdatedEntityEventArgs(TEntity entity, TUpdateDTO updateDTO) : base(entity, updateDTO)
    { }
}

public abstract class ModifiedEntityEventArgs<TEntity, TDTO> : EventArgs
{
    public TEntity Entity { get; }
    public TDTO DTO { get; }

    protected ModifiedEntityEventArgs(TEntity entity, TDTO dto)
    {
        Entity = entity;
        DTO = dto;
    }
}