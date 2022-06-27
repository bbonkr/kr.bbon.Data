using System;

namespace kr.bbon.Data.Abstractions.Entities
{
    /// <summary>
    /// Base class of Entity classes.
    /// </summary>
    public abstract class EntityBase : IEntity
    {
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? UpdatedAt { get; set; }
    }

    /// <summary>
    /// Base class of Entity classes.
    /// </summary>
    public abstract class EntityBase<TKey> : EntityBase, IEntityHasIdentifier<TKey>
    {
        public TKey Id { get; set; }

        public virtual bool IsKeyValueGeneratedOnAdd() => true;
    }

    public abstract class EntityBase<TKey, TKeyConversion> : EntityBase<TKey>, IEntityHasIdentifier<TKey, TKeyConversion>  //EntityBase, IEntityHasIdentifier<TKey, TKeyConversion>
    {
        //public TKey Id { get; set; }

        public Type ConversionType { get => typeof(TKeyConversion); }

    }
}
