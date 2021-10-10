using System;

namespace kr.bbon.Data
{
    /// <summary>
    /// Base class of Entity classes.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Represent whether entry was deleted.
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Represent when entry has been created.
        /// </summary>
        DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Represent when entry has been updated latest.
        /// </summary>
        DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// If use soft-deletion feature, Represent when entry has been deleted
        /// </summary>
        DateTimeOffset? DeletedAt { get; set; }
    }

    /// <summary>
    /// Base class of Entity classes.
    /// </summary>
    public abstract class Entity : IEntity
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
