using System;

namespace kr.bbon.Data.Abstractions.Entities
{
    /// <summary>
    /// Base class of Entity classes.
    /// </summary>
    public abstract class EntitySupportSoftDeletionBase : EntityBase, IEntitySupportSoftDeletion
    {
        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }
    }

    public abstract class EntitySupportSoftDeletionBase<TKey> : EntityBase<TKey>, IEntitySupportSoftDeletion
    {
        public bool IsDeleted { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }
    }

    public abstract class EntitySupportSoftDeletionBase<TKey, TKeyConversion> : EntitySupportSoftDeletionBase<TKey>, IEntityHasIdentifier<TKey, TKeyConversion>
    {
        public Type ConversionType { get => typeof(TKeyConversion); }
    }
}
