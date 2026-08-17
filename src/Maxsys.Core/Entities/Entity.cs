namespace Maxsys.Core.Entities;

public abstract class Entity;

/// <summary>
/// Entidade com chave única tipada.
/// </summary>
/// <typeparam name="TKey">
/// Tipo da chave primária. Deve ser um tipo escalar simples (<see cref="int"/>, <see cref="Guid"/>, <see cref="string"/>, etc.).<br/>
/// <b>Não use tipos compostos</b> (tuplas, records) como <typeparamref name="TKey"/> — o EF Core não consegue mapear
/// um único <see cref="Id"/> composto como chave primária. Para entidades com chave composta, herde de <see cref="Entity"/>
/// (sem TKey), declare as propriedades escalares separadamente e configure via
/// <c>builder.HasKey(x =&gt; new &#123; x.PropA, x.PropB &#125;)</c>.
/// </typeparam>
public abstract class Entity<TKey> : Entity, IKey<TKey>
{
    // TODO estudar comportamento quando required for aplicado.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public virtual TKey Id { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}