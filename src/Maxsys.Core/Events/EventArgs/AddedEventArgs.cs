namespace Maxsys.Core;

[Serializable]
public sealed class AddedEventArgs<TEntity, TCreateDTO> : EventArgs
{
    public TEntity AddedEntity { get; }
    public TCreateDTO CreateDTO { get; }

    public AddedEventArgs(TEntity entity, TCreateDTO createDTO)
    {
        AddedEntity = entity;
        CreateDTO = createDTO;
    }
}