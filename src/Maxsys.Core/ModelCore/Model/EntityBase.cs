using System;

namespace Maxsys.ModelCore
{
    /// <summary>
    /// Provides a base class for an Entity of key type <see cref="TKey"/>
    /// </summary>
    /// <typeparam name="TKey">Type of key (Id). <para/>Example: TKey=Guid => public Guid Id { get; protected set; }</typeparam>
    public abstract class EntityBase<TKey> : IEquatable<EntityBase<TKey>>
    {
        public virtual TKey Id { get; protected set; }

        public virtual bool Equals(EntityBase<TKey> entity)
        {
            return this.Id.Equals(entity.Id);
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as EntityBase<TKey>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Equals(compareTo);
        }

        public static bool operator ==(EntityBase<TKey> a, EntityBase<TKey> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EntityBase<TKey> a, EntityBase<TKey> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
}